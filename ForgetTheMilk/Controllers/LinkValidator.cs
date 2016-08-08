using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ForgetTheMilk.Controllers
{
    public class LinkValidator : ILinkValidator
    {
        public void Validate(string link)
        {
            var request = WebRequest.CreateHttp(link);
            request.Method = "HEAD";
            try
            {
                request.GetResponse();
            }
            catch (WebException exception)
            {
                throw new ApplicationException("Invalid Link " + link);
            }
        }
    }

    public interface ILinkValidator
    {
        void Validate(string link);
    }
}