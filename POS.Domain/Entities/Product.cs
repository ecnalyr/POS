using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using POS.Domain.Properties;
using POS.Domain.Validators;

namespace POS.Domain.Entities
{
    public class Product
    {
        /// <summary>
        /// Gets or sets the entity ID of a product
        /// </summary>
        /// <value>
        /// An integer identifying the entity.
        /// </value>
        [HiddenInput(DisplayValue = false)]
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the product
        /// </summary>
        /// <value>
        /// A string.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "ProductNameRequired",
            ErrorMessageResourceType = typeof (Resources))]
        [Display(Name = "ProductNameLebelText", ResourceType = typeof (Resources))]
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the description of the product
        /// </summary>
        /// <value>
        /// A string.
        /// </value> 
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "ProductDescriptionRequired",
            ErrorMessageResourceType = typeof (Resources))]
        [DataType(DataType.MultilineText)]
        [Display(Name = "ProductDescriptionLabelText", ResourceType = typeof (Resources))]
        public string Description { get; set; }


        /// <summary>
        /// Gets or sets the price of the product
        /// </summary>
        /// <value>
        /// A positive decimal.
        /// </value>
        [Required(ErrorMessageResourceName = "ProductPriceRequired", ErrorMessageResourceType = typeof (Resources))]
        [Range(0.01, double.MaxValue, ErrorMessageResourceName = "ProductPriceMustBePositiveError",
            ErrorMessageResourceType = typeof (Resources))]
        [Display(Name = "ProductPriceLabelText", ResourceType = typeof (Resources))]
        public decimal Price { get; set; }


        /// <summary>
        /// Gets or sets the entity Id of the category
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the virtual category
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// Gets or sets the ImageData
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// Gets or sets the ImageMimeType
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        [StringLength(100)]
        public string ImageMimeType { get; set; }

        /// <summary>
        /// Gets or sets the entity Id of the Establishment at which the Product is sold
        /// </summary>
        public int EstablishmentId { get; set; }

        /// <summary>
        /// Gets or sets the virtual Establishment
        /// </summary>
        public virtual Establishment Establishment { get; set; }
    }
}