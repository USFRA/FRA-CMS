// Copyright (c) 2007 Adrian Godong, Ben Maurer, Mike Hatalski, Derik Whittaker, Steven Carta
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Net;
using System.Web.Mvc;
using System.Configuration;
using System;

namespace RadCms.Recaptcha
{
    public static class RecaptchaControlMvc
    {
        private static string _publicKey;
        private static string _privateKey;
        private static bool _skipRecaptcha;
        //private static bool _overrideSecureMode;
        private static IWebProxy _proxy;

        public static string PublicKey
        {
            get { return _publicKey; }
            set { _publicKey = value; }
        }

        public static string PrivateKey
        {
            get { return _privateKey; }
            set { _privateKey = value; }
        }

        public static bool SkipRecaptcha
        {
            get { return _skipRecaptcha; }
            set { _skipRecaptcha = value; }
        }

        public static IWebProxy Proxy
        {
            get { return _proxy; }
            set { _proxy = value; }
        }

        static RecaptchaControlMvc()
        {
            _publicKey = ConfigurationManager.AppSettings["RecaptchaPublicKey"];
            _privateKey = ConfigurationManager.AppSettings["RecaptchaPrivateKey"];
            if (!bool.TryParse(ConfigurationManager.AppSettings["RecaptchaSkipValidation"], out _skipRecaptcha))
            {
                _skipRecaptcha = false;
            }
        }

        public class CaptchaValidatorAttribute : ActionFilterAttribute
        {
            private const string RESPONSE_FIELD_KEY = "g-recaptcha-response";

            private RecaptchaResponse _recaptchaResponse;

            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {

                if (SkipRecaptcha)
                {
                    filterContext.ActionParameters["captchaValid"] = true;
                    filterContext.ActionParameters["captchaErrorMessage"] = "Validation skipped";
                }
                else
                {
                    RecaptchaValidator validator = new RecaptchaValidator();
                    validator.PrivateKey = PrivateKey;
                    validator.RemoteIP = filterContext.HttpContext.Request.UserHostAddress;
                    validator.Response = filterContext.HttpContext.Request.Form[RESPONSE_FIELD_KEY];
                    validator.Proxy = _proxy;

                    if (string.IsNullOrEmpty(validator.Response))
                    {
                        this._recaptchaResponse = RecaptchaResponse.InvalidResponse;
                    }
                    else
                    {
                        this._recaptchaResponse = validator.Validate();
                    }

                    // this will push the result values into a parameter in our Action
                    filterContext.ActionParameters["captchaValid"] = _recaptchaResponse.IsValid;
                    filterContext.ActionParameters["captchaErrorMessage"] = _recaptchaResponse.ErrorMessage;
                }

                base.OnActionExecuting(filterContext);
            }
        }

        public static MvcHtmlString ReCAPTCHA(this HtmlHelper helper)
        {
            if (string.IsNullOrEmpty(_publicKey) || string.IsNullOrEmpty(_privateKey))
            {
                throw new ApplicationException("reCAPTCHA needs to be configured with a public & private key.");
            }

            var format = "<script src='https://www.google.com/recaptcha/api.js'></script>" +
                "<div class=\"g-recaptcha\" data-sitekey=\"{0}\"></div>";

            return MvcHtmlString.Create(string.Format(format, _publicKey));
        }
    }
}