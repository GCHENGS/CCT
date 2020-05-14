using CCT.Resource.Helpers.InterFace;

namespace CCT.Resource.Helpers
{
    public class PlugToUpper:Iplug
    {
        public string ProcessText(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return string.Empty;
            return str.ToUpper();
        }
    }
}
