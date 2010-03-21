
namespace Snap
{
    /// <summary>
    /// Outlines a runtime callable method.
    /// </summary>
    public class MethodSignature
    {
        /// <summary>
        /// Gets or sets the type format.
        /// </summary>
        /// <value>The type format.</value>
        public string TypeFormat { get; set; }
        /// <summary>
        /// Gets or sets the method format.
        /// </summary>
        /// <value>The method format.</value>
        public string MethodFormat { get; set; }
        /// <summary>
        /// Gets or sets the parameter format.
        /// </summary>
        /// <value>The parameter format.</value>
        public string ParameterFormat { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the type is generic.
        /// </summary>
        /// <value><c>true</c> if the type is generic; otherwise, <c>false</c>.</value>
        public bool TypeIsGeneric { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the method is generic.
        /// </summary>
        /// <value><c>true</c> if the method is generic; otherwise, <c>false</c>.</value>
        public bool MethodIsGeneric { get; set; }
    }
}
