using System.IO;

namespace Congregation.Common.Helpers
{
    public interface IFilesHelper
    {
        byte[] ReadFully(Stream input);
    }

}
