using System.Globalization;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Dal;

internal static class Config
{
    static string s_data_config_xml = "data-config";
    internal static int NextTaskId
    {
        get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId");
        set => XMLTools.SetNextId(s_data_config_xml, "NextTaskId", value);
    }
    internal static int NextDependencyId
    {
        get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId");
        set => XMLTools.SetNextId(s_data_config_xml, "NextDependencyId", value);
    }

    internal static DateTime? GetDate(string elementName)
    {
        XElement root = XMLTools.LoadListFromXMLElement(@"..\xml\" + s_data_config_xml);
        string dateStr = root.Element(elementName)!.Value;

        if (string.IsNullOrEmpty(dateStr))
            return null;
        DateTime date = DateTime.ParseExact(dateStr, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        return date;
    }

    internal static void SetDate(string elementName, DateTime? date)
    {
        string path = @"..\xml\" + s_data_config_xml;
        XElement root = XMLTools.LoadListFromXMLElement(path);
        root.Element(elementName)!.ReplaceWith(new XElement(elementName, date.ToString()));
        XMLTools.SaveListToXMLElement(root, path);
    }
}
