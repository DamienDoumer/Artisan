using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Used for data validation
/// </summary>
namespace Artisan.MVVMShared
{
    public class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo
    {
        /// <summary>
        /// This dictionary contains errors which could occure Here
        /// </summary>
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        /// <summary>
        /// Tells us if there is an error which occured
        /// </summary>
        public bool HasErrors
        {
            get
            {
                return _errors.Count > 0;
            } 
        }

        /// <summary>
        /// Event Raised when theer is anew error in aproperty
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>
        /// Gets errors for the property and returns the list of errors
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>List of errors gotten</returns>
        public IEnumerable GetErrors(string propertyName)
        {
            ///if the poperty contains an error, return the error's name else return null
            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName];
            else
                return null;
        }

        protected override void SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null)
        {
            base.SetProperty<T>(ref member, val, propertyName);

        }

        private void ValidateProperty<T>(string propertyName, T value)
        {
            var results = new List<ValidationResult>();
            ///Call a given object, 
            ValidationContext context = new ValidationContext(this);
            ///Say what property in the object is being validated
            context.MemberName = propertyName;
            ///Call a method to evaluate the object
            /// Goes throught the object, look for errors, if any is found,
            /// add it to the result collection.
            Validator.TryValidateProperty(value, context, results);

            ///if there are any results(errors)
            if (results.Any())
            {
                ///Add the errors in set the result as the error for the property 
                /// with the given name.
                _errors[propertyName] = results.Select(c => c.ErrorMessage).ToList();
            }
            else
            {
                ///else remove the property fromt eh dictionary cause there is no
                /// error a ssociated to it
                _errors.Remove(propertyName);
            }
            ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
