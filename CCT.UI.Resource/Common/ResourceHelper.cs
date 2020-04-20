using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CCT.UI.Resource.Common
{
    public class ResourceHelper
    {
        public static string FindStringResource(string key)
        {
            try
            {
                var findResource = Application.Current.FindResource(key);
                if (findResource != null)
                    return findResource.ToString();
                return string.Empty;
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                return string.Empty;
            }
            catch(NullReferenceException)
            {
                return string.Empty;
            }
        }

        public static object FindResource(string key)
        {
            try
            {
                return Application.Current.FindResource(key);
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                return null;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static object FindImageResource(string key)
        {
            try
            {
                return Application.Current.FindResource(key).ToString();
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                return null;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public static object FindColorResource(string key)
        {
            try
            {
                return Application.Current.FindResource(key);
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                return null;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
    }
}
