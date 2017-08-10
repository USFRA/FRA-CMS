using System;

namespace RadCms.Web.Areas.ImageLibrary.Helpers
{
    internal class NodeHelper
    {
        public static string GetNameFromPath(string path)
        {
            var folders = path.Split(new char[] { '/' });
            return folders[folders.Length - 1];
        }
        public static int GetNodeIdFromPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return 1;
            }

            var folders = path.Split(new char[] { '/' });
            if (folders.Length == 0 || string.IsNullOrEmpty(folders[0]))
            {
                return 1;
            }
            try
            {
                var nodeId = Int32.Parse(folders[0]);
                return nodeId;
            }
            catch (Exception)
            {
                return 1;
            }
        }
    }
}