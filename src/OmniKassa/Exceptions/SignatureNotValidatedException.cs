
namespace OmniKassa.Exceptions
{
	/// <summary>
	/// Exception thrown when a data property was requested but the signature in its parent class was not yet validated
	/// </summary>
	public class SignatureNotValidatedException : RabobankSdkException
	{
		/// <summary>
		/// Initializes a SignatureNotValidatedException
		/// </summary>
		public SignatureNotValidatedException() :
			base("Signature must first be validated using ValidateSignature()")
		{
			
		}
	}
}
