// ----------------------------------------------------------------------------------
// Copyright © 2017, Oleg Lobakov, Contacts: <oleg.lobakov@gmail.com>
// Licensed under the GNU GENERAL PUBLIC LICENSE, Version 3.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// https://github.com/OlegLobakov/WarehouseControlSystem/blob/master/LICENSE
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using PCLStorage;
using System.Threading.Tasks;

namespace WarehouseControlSystem
{
    /// <summary>
    /// PCLStorage GitHub Example
    /// </summary>
    public static class Storage
    {
        public async static Task<bool> IsFileExistAsync(this string fileName, IFolder rootFolder)
        {
            // get hold of the file system  
            IFolder folder = rootFolder ?? FileSystem.Current.LocalStorage;
            ExistenceCheckResult folderexist = await folder.CheckExistsAsync(fileName).ConfigureAwait(false);
            // already run at least once, don't overwrite what's there  
            if (folderexist == ExistenceCheckResult.FileExists)
            {
                return true;
            }
            return false;
        }

        public async static Task<bool> IsFolderExistAsync(this string folderName, IFolder rootFolder)
        {
            // get hold of the file system  
            IFolder folder = rootFolder ?? FileSystem.Current.LocalStorage;
            ExistenceCheckResult folderexist = await folder.CheckExistsAsync(folderName).ConfigureAwait(false);
            // already run at least once, don't overwrite what's there  
            if (folderexist == ExistenceCheckResult.FolderExists)
            {
                return true;
            }
            return false;
        }

        public async static Task<IFolder> CreateFolder(this string folderName, IFolder rootFolder)
        {
            IFolder folder = rootFolder ?? FileSystem.Current.LocalStorage;
            folder = await folder.CreateFolderAsync(folderName, CreationCollisionOption.ReplaceExisting).ConfigureAwait(false);
            return folder;
        }

        public async static Task<IFile> CreateFile(this string filename, IFolder rootFolder)
        {
            IFolder folder = rootFolder ?? FileSystem.Current.LocalStorage;
            IFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting).ConfigureAwait(false);
            return file;
        }

        public async static Task<bool> WriteTextAllAsync(this string filename, string content, IFolder rootFolder)
        {
            IFile file = await filename.CreateFile(rootFolder).ConfigureAwait(false);
            await file.WriteAllTextAsync(content);
            return true;
        }

        public async static Task<string> ReadAllTextAsync(this string fileName, IFolder rootFolder)
        {
            string content = "";
            IFolder folder = rootFolder ?? FileSystem.Current.LocalStorage;
            bool exist = await fileName.IsFileExistAsync(folder).ConfigureAwait(false);
            if (exist == true)
            {
                IFile file = await folder.GetFileAsync(fileName).ConfigureAwait(false);
                content = await file.ReadAllTextAsync().ConfigureAwait(false);
            }
            return content;
        }
        public async static Task<bool> DeleteFile(this string fileName, IFolder rootFolder)
        {
            IFolder folder = rootFolder ?? FileSystem.Current.LocalStorage;
            bool exist = await fileName.IsFileExistAsync(folder).ConfigureAwait(false);
            if (exist == true)
            {
                IFile file = await folder.GetFileAsync(fileName).ConfigureAwait(false);
                await file.DeleteAsync();
                return true;
            }
            return false;
        }
        public async static Task SaveImage(this byte[] image, String fileName, IFolder rootFolder)
        {
            // get hold of the file system  
            IFolder folder = rootFolder ?? FileSystem.Current.LocalStorage;

            // create a file, overwriting any existing file  
            IFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting).ConfigureAwait(false);

            // populate the file with image data  
            using (System.IO.Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite).ConfigureAwait(false))
            {
                stream.Write(image, 0, image.Length);
            }
        }

        public async static Task<byte[]> LoadImage(this byte[] image, String fileName, IFolder rootFolder)
        {
            // get hold of the file system  
            IFolder folder = rootFolder ?? FileSystem.Current.LocalStorage;

            //open file if exists  
            IFile file = await folder.GetFileAsync(fileName);
            //load stream to buffer  
            using (System.IO.Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite).ConfigureAwait(false))
            {
                long length = stream.Length;
                byte[] streamBuffer = new byte[length];
                stream.Read(streamBuffer, 0, (int)length);
                return streamBuffer;
            }
        }
    }
}
