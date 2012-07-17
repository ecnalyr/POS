// -----------------------------------------------------------------------

// <copyright file="TextLineValidatorAtribute.cs" company="Microsoft">

// TODO: Update copyright text.

// </copyright>

// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using POS.Domain.Properties;

namespace POS.Domain.Validators
{
    #region

    

    #endregion

    /// <summary>
    ///   Only alpha-numeric characters and [.,_-;] are allowed.
    /// </summary>
    public class TextLineInputValidatorAtribute : RegularExpressionAttribute, IClientValidatable
    {
        #region Constructors and Destructors

        public TextLineInputValidatorAtribute()
            : base(Resources.TextLineInputValidatorRegEx)
        {
            ErrorMessage = Resources.InvalidInputCharacter;
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
                           {
                               ErrorMessage = Resources.InvalidInputCharacter,
                               ValidationType = "textlineinput"
                           };

            rule.ValidationParameters.Add("pattern", Resources.TextLineInputValidatorRegEx);

            return new List<ModelClientValidationRule> {rule};
        }

        #endregion
    }
}