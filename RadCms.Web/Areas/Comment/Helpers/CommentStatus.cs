using System.Collections.Generic;

namespace RadCms.Web.Areas.Comment.Helpers
{
    internal static class CommentStatus
    {
        public const int Active = 1;
        public const int Deleted = 0;
        public const int Pending = -1;

        public static Dictionary<string, int?> List = new Dictionary<string, int?>{
            {"ALL", null},
            {"Published", Active},
            {"Rejected", Deleted},
            {"Pending Review", Pending}
        };
    }
}