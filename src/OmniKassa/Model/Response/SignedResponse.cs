using System;
using OmniKassa.Exceptions;

namespace OmniKassa.Model.Response
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SignedResponse : Signable
    {
        /// <summary>
        /// Whether or not the signature is valid. Only set once <see cref="ValidateSignature(string)"/> is called.
        /// </summary>
        protected bool isSignatureValid = false;

        /// <summary>
        /// Initializes an empty SignedResponse
        /// </summary>
        protected SignedResponse()
        {
            
        }

        /// <summary>
        /// Initializes a SignedResponse with given signature
        /// </summary>
        /// <param name="signature">Signature</param>
        protected SignedResponse(String signature)
        {
            this.Signature = signature;
        }

        /// <summary>
        /// Validates if the signature is identical to the signature from the API response
        /// </summary>
        /// <param name="signingKey">Signing key</param>
        public void ValidateSignature(String signingKey)
        {
            byte[] signingKeyBytes = Convert.FromBase64String(signingKey);
            ValidateSignature(signingKeyBytes);
        }

        /// <summary>
        /// Validates if the signature is identical to the signature from the API response
        /// </summary>
        /// <param name="signingKey">Signing key</param>
        public void ValidateSignature(byte[] signingKey)
        {
            String calculatedSignature = CalculateSignature(GetSignatureData(), signingKey);
            if (calculatedSignature.Equals(Signature))
            {
                isSignatureValid = true;
            }
            else
            {
                throw new IllegalSignatureException();
            }
        }
    }
}
