using System.IO;

namespace VersionControl_Desktop {
    public struct FolderTransportInfo {
        //整个folder都上传，那必然是需要新建整个folder啊，不考虑是否覆盖的问题
        public DirectoryInfo from;
        public string to;
        public FolderTransportInfo(DirectoryInfo f, string t) {
            this.from = f;
            this.to = t;
        }
    }
    public struct FileTransportInfo {
        public FileInfo from;
        public string to;
        public bool cover;//是否覆盖to路径下已经有的旧文件

        public FileTransportInfo(FileInfo f, string t) {
            this.from = f;
            this.to = t;
            this.cover = false;
        }

        public FileTransportInfo(FileInfo f, string t, bool cover) {
            this.from = f;
            this.to = t;
            this.cover = cover;
        }
    }


}