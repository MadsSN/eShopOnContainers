using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Template1.API.Model
{
    public class Template1 : IValidatableObject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Template1() { }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name.Length > 3)
            {
                yield return new ValidationResult(
                    "Name must be longer than three charactors",
                    new[] { nameof(Name)});
            }
        }
    }
}
