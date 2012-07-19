using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using POS.Domain.Properties;
using POS.Domain.Validators;

namespace POS.Domain.Entities
{
    public class ParentCategory
    {
        /// <summary>
        /// Gets or sets the entity ID of a category
        /// </summary>
        /// <value>
        /// An integer identifying the entity.
        /// </value>
        [HiddenInput(DisplayValue = false)]
        public int ParentCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the parent category
        /// </summary>
        /// <value>
        /// A one-word string, only alpha-numeric characters and [.,_-;] are allowed
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "ProductParentCategoryRequired",
            ErrorMessageResourceType = typeof(Resources))]
        [StringLength(40, ErrorMessageResourceName = "ProductParentCategoryLengthError",
            ErrorMessageResourceType = typeof(Resources))]
        [TextLineInputValidatorAtribute]
        [Display(Name = "ProductParentCategoryLabelText", ResourceType = typeof(Resources))]
        public string Name { get; set; }
    }
}
