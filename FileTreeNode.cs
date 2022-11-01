using System;
using System.IO;

public class FolderTreeNode {
    public string nodePath;
    public DirectoryInfo folderInfo;

    public List<FileInfo> fileInfos_children = new List<FileInfo>();
    public List<DirectoryInfo> folderInfos_children = new List<DirectoryInfo>();

    public FolderTreeNode(string nodePath) {
        this.nodePath = nodePath;
        this.folderInfo = new DirectoryInfo(nodePath);

        //把这个节点里面包含的所有folder和file信息都给列出来
        if (this.folderInfo != null) {
            DirectoryInfo[] folderChildrenInfos = this.folderInfo.GetDirectories();
            FileInfo[] fileChildrenInfos = this.folderInfo.GetFiles();

            this.folderInfos_children = new List<DirectoryInfo>();
            foreach (DirectoryInfo folderChildInfo in folderChildrenInfos) {
                this.folderInfos_children.Add(folderChildInfo);
            }

            this.fileInfos_children = new List<FileInfo>();
            foreach (FileInfo fileChildInfo in fileChildrenInfos) {
                this.fileInfos_children.Add(fileChildInfo);
            }
        } else {
            Console.WriteLine("found null directoryInfo while creating folderTreeNode:", nodePath);
        }
    }
    public FolderTreeNode(DirectoryInfo directoryInfo) {
        if (directoryInfo == null) {
            throw new ArgumentNullException("found null directoryInfo while creating folderTreeNode:");
        }

        this.folderInfo = directoryInfo;
        this.nodePath = directoryInfo.FullName;

        //把这个节点里面包含的所有folder和file信息都给列出来
        if (this.folderInfo != null) {
            DirectoryInfo[] folderChildrenInfos = this.folderInfo.GetDirectories();
            FileInfo[] fileChildrenInfos = this.folderInfo.GetFiles();

            this.folderInfos_children = new List<DirectoryInfo>();
            foreach (DirectoryInfo folderChildInfo in folderChildrenInfos) {
                this.folderInfos_children.Add(folderChildInfo);
            }

            this.fileInfos_children = new List<FileInfo>();
            foreach (FileInfo fileChildInfo in fileChildrenInfos) {
                this.fileInfos_children.Add(fileChildInfo);
            }
        } else {
            Console.WriteLine("found null directoryInfo while creating folderTreeNode:", directoryInfo.FullName);
        }

    }

    public List<string> getAllChildrenFileNames() {
        List<string> childrenFileNames = new List<string>();

        foreach (FileInfo fileInfo in this.fileInfos_children) {
            childrenFileNames.Add(fileInfo.Name);
        }
        return childrenFileNames;
    }

    public List<string> getAllChildrenFolderNames() {
        List<string> childrenFolderNames = new List<string>();

        foreach (DirectoryInfo folderInfo in this.folderInfos_children) {
            childrenFolderNames.Add(folderInfo.Name);
        }
        return childrenFolderNames;
    }

    public int getChildFilesCount() {
        return this.fileInfos_children.Count;
    }
    public int getChildFoldersCount() {
        return this.folderInfos_children.Count;
    }

    public bool isEmptyFolder() {
        int childrenFileCount = getChildFilesCount();
        int childrenFolderCount = getChildFoldersCount();

        if (childrenFileCount + childrenFolderCount > 0) {
            return false;
        }
        return true;
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

public struct FolderTransportInfo {
    //整个folder都上传，那必然是需要新建整个folder啊，不考虑是否覆盖的问题
    public DirectoryInfo from;
    public string to;
    public FolderTransportInfo(DirectoryInfo f, string t) {
        this.from = f;
        this.to = t;
    }
}
