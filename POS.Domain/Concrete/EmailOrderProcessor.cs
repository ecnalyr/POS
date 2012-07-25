using System.Net;
using System.Net.Mail;
using System.Text;
using POS.Domain.Abstract;
using POS.Domain.Entities;

namespace POS.Domain.Concrete
{
    public class EmailSettings
    {
        #region Fields

        public string FileLocation = @"c:\pos_emails";
        public string MailFromAddress = "ccdiocese@gmail.com";
        public string MailToAddress = "ecnalyr@gmail.com";
        public string Password = "pas5word";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 587;
        public bool UseSsl = true;
        public string Username = "ccdiocese@gmail.com";
        public bool WriteAsFile;

        #endregion
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        #region Fields

        private readonly EmailSettings emailSettings;

        #endregion

        #region Constructoras and Destructors

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        #endregion

        #region Public Methods and Operators

        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body =
                    new StringBuilder().AppendLine("A new order has been submitted").AppendLine("---").AppendLine(
                        "Items:");
                foreach (CartLine line in cart.Lines)
                {
                    decimal subtotal = line.Product.Price*line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c}", line.Quantity, line.Product.Name, subtotal);
                }

                body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Ship to:")
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.Line1)
                    .AppendLine(shippingInfo.Line2 ?? string.Empty)
                    .AppendLine(shippingInfo.Line3 ?? string.Empty)
                    .AppendLine(shippingInfo.City)
                    .AppendLine(shippingInfo.State ?? string.Empty)
                    .AppendLine(shippingInfo.Country)
                    .AppendLine(shippingInfo.Zip)
                    .AppendLine("---").AppendFormat(
                                    "Gift wrap: {0}", shippingInfo.GiftWrap ? "Yes" : "No");
                                    var mailMessage = new MailMessage(
                                    emailSettings.MailFromAddress,
                                    emailSettings.MailToAddress,
                                    "New order submitted!",
                                    body.ToString());

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(mailMessage);
            }
        }

        #endregion
    }
}