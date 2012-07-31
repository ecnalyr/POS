using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using POS.Domain.Properties;
using POS.Domain.Validators;

namespace POS.Domain.Model
{
    public class Category
    {
        /// <summary>
        /// Gets or sets the entity ID of a category
        /// </summary>
        /// <value>
        /// An integer identifying the entity.
        /// </value>
        [HiddenInput(DisplayValue = false)]
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the category
        /// </summary>
        /// <value>
        /// A one-word string, only alpha-numeric characters and [.,_-;] are allowed
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "ProductCategoryRequired",
            ErrorMessageResourceType = typeof(Resources))]
        [StringLength(40, ErrorMessageResourceName = "ProductCategoryLengthError",
            ErrorMessageResourceType = typeof(Resources))]
        [TextLineInputValidatorAtribute]
        [Display(Name = "ProductCategoryLabelText", ResourceType = typeof(Resources))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the entity Id of the parent category
        /// </summary>
        public int ParentCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the virtual parent category
        /// </summary>
        public virtual ParentCategory ParentCategory { get; set; }
    }
}
