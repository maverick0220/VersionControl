using System;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using VersionControl_Desktop;
using System.Linq;

// to do:
// 传输文件的时候不是多线程的，也没考虑到进程占用文件的情况（按理来说不应该有这个情况


public class VersionControl {

    public static void CreateTestFileTree_D() {
        // remove pre-created folders
        Directory.Delete("D:\\versionControl\\A-from", true);
        Directory.Delete("D:\\versionControl\\A-to", true);

        // from-tree
        Directory.CreateDirectory("D:\\versionControl\\A-from");
        Directory.CreateDirectory("D:\\versionControl\\A-from\\B1");
        Directory.CreateDirectory("D:\\versionControl\\A-from\\B2");
        Directory.CreateDirectory("D:\\versionControl\\A-from\\B1\\C1");
        Directory.CreateDirectory("D:\\versionControl\\A-from\\B1\\C2");


        File.Create("D:\\versionControl\\A-from\\b1.txt");
        File.Create("D:\\versionControl\\A-from\\b2.txt");
        File.Create("D:\\versionControl\\A-from\\b3.txt");
        File.Create("D:\\versionControl\\A-from\\B1\\C1\\d1.txt");
        File.Create("D:\\versionControl\\A-from\\B1\\C1\\d2.txt");
        File.Create("D:\\versionControl\\A-from\\B1\\C2\\d3.txt");

        // to-tree
        Directory.CreateDirectory("D:\\versionControl\\A-to");
        Directory.CreateDirectory("D:\\versionControl\\A-to\\B1");
        Directory.CreateDirectory("D:\\versionControl\\A-to\\B2");
        Directory.CreateDirectory("D:\\versionControl\\A-to\\B1\\C1");
        Directory.CreateDirectory("D:\\versionControl\\A-to\\B1\\C2");

        File.Create("D:\\versionControl\\A-to\\b1.txt");
        //File.Create("D:\\versionControl\\A-to\\b2.txt");
        File.Create("D:\\versionControl\\A-to\\b3.txt");
        //File.Create("D:\\versionControl\\A-to\\B1\\C1\\d1.txt");
        File.Create("D:\\versionControl\\A-to\\B1\\C1\\d2.txt");
        File.Create("D:\\versionControl\\A-to\\B1\\C2\\d3.txt");
    }
    public static void CreateTestFileTree_E() {
        // remove pre-created folders
        Directory.Delete(@"E:\versionControl\A-from", true);
        Directory.Delete(@"E:\versionControl\A-to", true);

        // from-tree
        Directory.CreateDirectory("E:\\versionControl\\A-from");
        Directory.CreateDirectory("E:\\versionControl\\A-from\\B1");
        Directory.CreateDirectory("E:\\versionControl\\A-from\\B2");
        Directory.CreateDirectory("E:\\versionControl\\A-from\\B1\\C1");
        Directory.CreateDirectory("E:\\versionControl\\A-from\\B1\\C2");

        File.Create("E:\\versionControl\\A-from\\b1.txt");
        File.Create("E:\\versionControl\\A-from\\b2.txt");
        File.Create("E:\\versionControl\\A-from\\b3.txt");
        File.Create("E:\\versionControl\\A-from\\B2\\c1.txt");
        File.Create("E:\\versionControl\\A-from\\B1\\C1\\d1.txt");
        File.Create("E:\\versionControl\\A-from\\B1\\C1\\d2.txt");
        File.Create("E:\\versionControl\\A-from\\B1\\C2\\d3.txt");

        // to-tree
        Directory.CreateDirectory("E:\\versionControl\\A-to");
        Directory.CreateDirectory("E:\\versionControl\\A-to\\B1");
        Directory.CreateDirectory("E:\\versionControl\\A-to\\B2");
        Directory.CreateDirectory("E:\\versionControl\\A-to\\B1\\C1");
        Directory.CreateDirectory("E:\\versionControl\\A-to\\B1\\C2");

        File.Create("E:\\versionControl\\A-to\\b1.txt");
        //File.Create("E:\\versionControl\\A-to\\b2.txt");
        File.Create("E:\\versionControl\\A-to\\b3.txt");
        //File.Create("E:\\versionControl\\A-to\\B1\\C1\\d1.txt");
        File.Create("E:\\versionControl\\A-to\\B1\\C1\\d2.txt");
        File.Create("E:\\versionControl\\A-to\\B1\\C2\\d3.txt");
    }

    public static void Test(string a, string b) {
        Console.WriteLine("vc did get a:" + a + " and b:" + b);
    }

    public static void TransportAll(string fromPath, string toPath) {
        Console.WriteLine(fromPath, toPath);

        DirectoryInfo fromPathInfo = new DirectoryInfo(fromPath);
        FolderTransportInfo info = new FolderTransportInfo(fromPathInfo, toPath);

        List<FolderTransportInfo> infoList = new List<FolderTransportInfo>();
        infoList.Add(info);
        TransportFolders(infoList);
    }

    public static void SearchAndTransport(string fromPath, string toPath) {
        //string fromPath = "E:\\versionControl\\A-from";
        //string toPath = "E:\\versionControl\\A-to";

        // 1.set 2 trees
        FolderTreeNode newFolder = new FolderTreeNode(fromPath);
        FolderTreeNode oldFolder = new FolderTreeNode(toPath);

        List<(FolderTreeNode, FolderTreeNode)> foldersQueueForCompare = new List<(FolderTreeNode, FolderTreeNode)>();
        foldersQueueForCompare.Add((newFolder, oldFolder));

        //只要foldersQueueForCompare里面还有要比对的路径这事儿就没完
        while (foldersQueueForCompare.Count > 0) {

            //Console.WriteLine("foldersQueueForCompare count: " + foldersQueueForCompare.Count.ToString());
            // 2.比对当前路径下的文件有哪些需要拷贝过去的，有的话就地拷贝了
            (newFolder, oldFolder) = foldersQueueForCompare.First();
            List<FileTransportInfo> fileTransportInfos = CompareFolderTreeNodes_childrenFiles(newFolder, oldFolder);
            if (fileTransportInfos.Count > 0) {
                Thread transportThread = new Thread(new ParameterizedThreadStart(TransportFiles));
                transportThread.Start((object)fileTransportInfos);
                //TransportFiles(fileTransportInfos);
            }

            // 3.比对当前路径下的文件夹有哪些需要拷贝过去的，有的话也就地拷贝了
            List<FolderTransportInfo> folderTransportInfos = new List<FolderTransportInfo>();
            List<(FolderTreeNode, FolderTreeNode)> folderToCompare = new List<(FolderTreeNode, FolderTreeNode)>();
            (folderTransportInfos, folderToCompare) = CompareFolderTreeNodes_childrenFolders(newFolder, oldFolder);
            if (folderTransportInfos.Count > 0) {
                TransportFolders(folderTransportInfos);
            }

            // 4.去掉这次检查对比过的folder，把这次检查发现的还需要继续检查的folder加入queue，
            foldersQueueForCompare.RemoveAt(0);
            foldersQueueForCompare.AddRange(folderToCompare);
        }

        Console.Write("did success\n");
    }


    private static (List<FolderTransportInfo>, List<(FolderTreeNode, FolderTreeNode)>) CompareFolderTreeNodes_childrenFolders(FolderTreeNode newFolder, FolderTreeNode oldFolder) {
        //只是比对目标oldFolder里是不是没有newFolder里的某些目录，没有就新建FolderTransportInfo，有就给扔进那个Queue里
        //传回去的东西：
        //List<FolderTransportInfo>：有哪些要拷贝的目录
        //List<(FolderTreeNode, FolderTreeNode)：有哪些是需要放进queue里继续向下检查的

        // 1.所有要进行paste的文件，从哪儿到哪儿的信息
        List<FolderTransportInfo> foldersToTransport = new List<FolderTransportInfo>();
        List<(FolderTreeNode, FolderTreeNode)> foldersToCompare = new List<(FolderTreeNode, FolderTreeNode)>();

        // 2.如果俩文件夹里面都没有文件夹，那就不需要在当前路径下进行文件夹的传输
        if (newFolder.isEmptyFolder() && oldFolder.isEmptyFolder()) {
            Console.WriteLine("CompareFolderTreeNodes_childrenFolders: " + oldFolder.folderInfo.FullName + "," + newFolder.folderInfo.FullName);
            Console.WriteLine("CompareFolderTreeNodes_childrenFolders return 0");
            return (foldersToTransport, foldersToCompare);
        }

        // 3.汇总一下oldFolder里面有哪些文件
        List<string> folderNames_oldFolder = oldFolder.getAllChildrenFolderNames();
        if (folderNames_oldFolder.Count == 0) {
            //此处的情况：目标点啥也没有，但是出发点有文件，那就直接把出发点的所有文件都给拷贝进空白的目标点
            foreach (DirectoryInfo folderInfo_new in newFolder.folderInfos_children) {
                foldersToTransport.Add(new FolderTransportInfo(folderInfo_new, oldFolder.nodePath));
            }
            return (foldersToTransport, foldersToCompare);
        }

        // 4.逐一比对新旧文件夹里的文件，看看是否需要paste
        foreach (DirectoryInfo folderInfo_newFolder in newFolder.folderInfos_children) {
            if (folderNames_oldFolder.Contains(folderInfo_newFolder.Name.ToString()) == false) {
                //如果文件在目标地点不存在，那这就是个新文件，得把新的文件复制过去
                foldersToTransport.Add(new FolderTransportInfo(folderInfo_newFolder, oldFolder.nodePath));
            } else {
                //如果文件存在，那就把俩文件都丢进queue里再对比
                FolderTreeNode childrenFolder_newFolder = new FolderTreeNode(folderInfo_newFolder);
                FolderTreeNode childrenFolder_oldFolder = new FolderTreeNode(oldFolder.folderInfos_children.Find(x => x.Name == folderInfo_newFolder.Name));
                foldersToCompare.Add((childrenFolder_newFolder, childrenFolder_oldFolder));
            }
        }

        return (foldersToTransport, foldersToCompare);
    }
    private static List<FileTransportInfo> CompareFolderTreeNodes_childrenFiles(FolderTreeNode newFolder, FolderTreeNode oldFolder) {
        // 1.所有要进行paste的文件，从哪儿到哪儿的信息
        List<FileTransportInfo> filesToTransfer = new List<FileTransportInfo>();

        // 2.如果俩文件夹里面都没有子文件夹，那就不需要在当前路径下进行文件夹的传输，之后比对当前路径下的folder中是否有需要传输的
        if (newFolder.getChildFilesCount() == oldFolder.getChildFilesCount() && newFolder.getChildFilesCount() == 0) {
            return filesToTransfer;
        }

        // 3.汇总一下oldFolder里面有哪些文件
        List<string> fileNames_oldFolder = oldFolder.getAllChildrenFileNames();
        if (fileNames_oldFolder.Count == 0) {
            //此处的情况：目标点啥也没有，但是出发点有文件，那就直接把出发点的所有文件都给拷贝进空白的目标点
            foreach (FileInfo fileInfo_new in newFolder.fileInfos_children) {
                filesToTransfer.Add(new FileTransportInfo(fileInfo_new, oldFolder.nodePath));
            }
            return filesToTransfer;
        }

        // 4.逐一比对新旧文件夹里的文件，看看是否需要paste
        foreach (FileInfo fileInfo_newFolder in newFolder.fileInfos_children) {
            if (fileNames_oldFolder.Contains(fileInfo_newFolder.Name.ToString()) == false) {
                //如果文件在目标地点不存在，那这就是个新文件，得把新的文件复制过去
                filesToTransfer.Add(new FileTransportInfo(fileInfo_newFolder, oldFolder.nodePath));
            } else {
                //如果文件存在，那就对比俩文件谁更新
                if (FileTimeStampCompare(fileInfo_newFolder, oldFolder.fileInfos_children.Find(x => x.Name == fileInfo_newFolder.Name))) {
                    filesToTransfer.Add(new FileTransportInfo(fileInfo_newFolder, oldFolder.nodePath, true));
                }
            }
        }
        return filesToTransfer;
    }

    private static void TransportFiles(object TransportInfo) {
        List<FileTransportInfo> transportInfo = (List<FileTransportInfo>)TransportInfo;
        foreach (FileTransportInfo info in transportInfo) {
            Console.WriteLine("File Transport: from =" + info.from.FullName + "= to =" + info.to + "\n");

            try {
                info.from.CopyTo(info.to + @"\\" + info.from.Name);
                //File.Copy(info.from, info.to, info.cover);
            } catch (Exception e) {
                // Catch exception if the file was already copied.
                Console.WriteLine("===TransportFiles: " + e.Message);
                Console.WriteLine("===FileTransportInfo: " + info.from + " to " + info.to + "\n");
            }

        }
    }
    private static void TransportFolders(List<FolderTransportInfo> transportInfo) {
        foreach (FolderTransportInfo info in transportInfo) {
            Console.WriteLine("Folder Transport: from =" + info.from.FullName + "= to =" + info.to + "\n");

            try {
                string destinationPath = info.to + @"\\" + info.from.Name;

                // 1.在目标地点新建folder
                Directory.CreateDirectory(destinationPath);

                // 2.把文件这种能拷贝进去的都拷贝进去
                List<FileTransportInfo> newFilesTransport = new List<FileTransportInfo>();
                foreach (FileInfo newFileInfo in info.from.GetFiles()) {
                    newFilesTransport.Add(new FileTransportInfo(newFileInfo, destinationPath));
                }

                Thread transportThread = new Thread(new ParameterizedThreadStart(TransportFiles));
                transportThread.Start((object)newFilesTransport);
                //TransportFiles(newFilesTransport);

                // 3.把子folder也弄进去，递归？
                List<FolderTransportInfo> newFoldersTransport = new List<FolderTransportInfo>();
                foreach (DirectoryInfo newFolderInfo in info.from.GetDirectories()) {
                    newFoldersTransport.Add(new FolderTransportInfo(newFolderInfo, destinationPath));
                }
                TransportFolders(newFoldersTransport);

            } catch (Exception e) {
                Console.WriteLine("===TransportFolders: " + e.Message);
                Console.WriteLine("===FolderTransportInfo: " + info.from + " to " + info.to + "\n");
            }

        }
    }

    private static bool FileTimeStampCompare(FileInfo newFile, FileInfo oldFile) {
        //true: start比target新，需要paste
        //false：start比target旧，不需要paste

        if (newFile == null || oldFile == null) {
            return true;
        }

        if (newFile.LastWriteTimeUtc > oldFile.LastWriteTimeUtc) {
            return true;
        }
        return false;
    }

    private static List<FileInfo> GetAllFilesInDirectory(string path) {
        List<FileInfo> listFiles = new List<FileInfo>(); //保存所有的文件信息  
        DirectoryInfo directory = new DirectoryInfo(path);

        //下层要是还有文件，那就给保存文件信息的array扩个容
        FileInfo[] fileInfoArray = directory.GetFiles();
        if (fileInfoArray.Length > 0) listFiles.AddRange(fileInfoArray);

        DirectoryInfo[] nextLayerDirectorys = directory.GetDirectories();
        foreach (DirectoryInfo nextLayerDirectory in nextLayerDirectorys) {
            DirectoryInfo directoryA = new DirectoryInfo(nextLayerDirectory.FullName);

            DirectoryInfo[] directoryArrayA = directoryA.GetDirectories();
            FileInfo[] fileInfoArrayA = directoryA.GetFiles();

            if (fileInfoArrayA.Length > 0) listFiles.AddRange(fileInfoArrayA);

            GetAllFilesInDirectory(nextLayerDirectory.FullName);//递归遍历  
        }
        return listFiles;
    }

    private static void PrintFileInfos(List<FileInfo> infos) {
        for (int i = 0; i < infos.Count; i++) {
            var createionTime = infos[i].CreationTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
            var lastWriteTime = infos[i].LastWriteTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
            var fileSize = infos[i].Length;
            Console.Write(infos[i] + ", " + createionTime + ", " + lastWriteTime + ", " + fileSize + "\n");
        }
    }

    private static void CheckPath(string path) {
        DirectoryInfo[] d = new DirectoryInfo(path).GetDirectories();
        Console.WriteLine(d.Length);
        foreach (DirectoryInfo dir in d) {
            Console.WriteLine(dir.FullName);
        }

        FileInfo[] f = new DirectoryInfo(path).GetFiles();
        Console.WriteLine(f.Length);
        foreach (FileInfo file in f) {
            Console.WriteLine(file.FullName);
        }
    }

    public static long GetAllFileCount(string path) {
        long fileCounts = 0;

        try {
            var files = Directory.GetFiles(path); //String数组类型
            fileCounts += files.Length;
            Console.WriteLine(files);

            //遍历文件夹
            var dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs) {
                fileCounts += GetAllFileCount(dir);
            }

        } catch (Exception e) { Console.WriteLine(e.Message); }

        return fileCounts;
    }
}

