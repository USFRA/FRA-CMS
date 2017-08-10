using System.Collections.Generic;

namespace RadCms.Services
{
    public interface ITreeModelService<TModel>
    {
        IEnumerable<TModel> GetChildren(int parentId);
        IEnumerable<TModel> GetChildrenFolders(int parentId);
    }
}
