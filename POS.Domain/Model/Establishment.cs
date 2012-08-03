using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace POS.Domain.Model
{
    public class Establishment
    {
        [HiddenInput(DisplayValue = false)]
        public int EstablishmentId { get; set; }

        public string Name { get; set; }

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
        /// Intend on having this return a list of Products sold by Establishment
        /// </summary>
        public virtual ICollection<Product> Products { get; set; }  
    }
}
