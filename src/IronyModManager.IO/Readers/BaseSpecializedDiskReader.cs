﻿// ***********************************************************************
// Assembly         : IronyModManager.IO
// Author           : Mario
// Created          : 02-13-2021
//
// Last Modified By : Mario
// Last Modified On : 10-29-2022
// ***********************************************************************
// <copyright file="BaseSpecializedDiskReader.cs" company="Mario">
//     Mario
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IronyModManager.DI;
using IronyModManager.IO.Common.Readers;
using IronyModManager.Shared;

namespace IronyModManager.IO.Readers
{
    /// <summary>
    /// Class BaseSpecializedDiskReader.
    /// Implements the <see cref="IronyModManager.IO.Common.Readers.IFileReader" />
    /// </summary>
    /// <seealso cref="IronyModManager.IO.Common.Readers.IFileReader" />
    public abstract class BaseSpecializedDiskReader : IFileReader
    {
        #region Properties

        /// <summary>
        /// Gets the search extension.
        /// </summary>
        /// <value>The search extension.</value>
        public abstract string SearchExtension { get; }

        /// <summary>
        /// Gets the search option.
        /// </summary>
        /// <value>The search option.</value>
        public abstract SearchOption SearchOption { get; }

        /// <summary>
        /// Gets the search pattern.
        /// </summary>
        /// <value>The search pattern.</value>
        public abstract string SearchPattern { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Determines whether this instance [can list files] the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if this instance [can list files] the specified path; otherwise, <c>false</c>.</returns>
        public virtual bool CanListFiles(string path)
        {
            return false;
        }

        /// <summary>
        /// Determines whether this instance can read the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="searchSubFolders">if set to <c>true</c> [search sub folders].</param>
        /// <returns><c>true</c> if this instance can read the specified path; otherwise, <c>false</c>.</returns>
        public virtual bool CanRead(string path, bool searchSubFolders = true)
        {
            return Directory.Exists(path) && path.EndsWith(SearchExtension, StringComparison.OrdinalIgnoreCase) && searchSubFolders;
        }

        /// <summary>
        /// Determines whether this instance [can read stream] the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if this instance [can read stream] the specified path; otherwise, <c>false</c>.</returns>
        public virtual bool CanReadStream(string path)
        {
            return false;
        }

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>IList&lt;System.String&gt;.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public virtual IEnumerable<string> GetFiles(string path)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <param name="file">The file.</param>
        /// <returns>System.ValueTuple&lt;Stream, System.Boolean, System.Nullable&lt;DateTime&gt;, Common.Readers.EncodingInfo&gt;.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public virtual (Stream, bool, DateTime?, EncodingInfo) GetStream(string rootPath, string file)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the total size.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>System.Int64.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public virtual long GetTotalSize(string path)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Reads the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="allowedPaths">The allowed paths.</param>
        /// <param name="searchSubFolders">if set to <c>true</c> [search sub folders].</param>
        /// <returns>IReadOnlyCollection&lt;IFileInfo&gt;.</returns>
        public virtual IReadOnlyCollection<IFileInfo> Read(string path, IEnumerable<string> allowedPaths = null, bool searchSubFolders = true)
        {
            return ReadInternal(Directory.GetFiles(path, SearchPattern, SearchOption), path);
        }

        /// <summary>
        /// Reads the internal.
        /// </summary>
        /// <param name="files">The files.</param>
        /// <param name="path">The path.</param>
        /// <returns>IReadOnlyCollection&lt;IFileInfo&gt;.</returns>
        protected virtual IReadOnlyCollection<IFileInfo> ReadInternal(string[] files, string path)
        {
            if (files?.Length > 0)
            {
                var result = new List<IFileInfo>();
                foreach (var file in files)
                {
                    var relativePath = file.Replace(path, string.Empty).Trim(Path.DirectorySeparatorChar);
                    var info = DIResolver.Get<IFileInfo>();
                    var fileInfo = new System.IO.FileInfo(file);
                    info.IsReadOnly = fileInfo.IsReadOnly;
                    info.Size = fileInfo.Length;
                    var content = File.ReadAllText(file);
                    info.FileName = relativePath;
                    info.IsBinary = false;
                    info.Content = content.SplitOnNewLine(false);
                    info.ContentSHA = content.CalculateSHA();
                    info.LastModified = fileInfo.LastWriteTime;
                    info.Encoding = file.GetEncodingInfo();
                    result.Add(info);
                }
                return result;
            }
            return null;
        }

        #endregion Methods
    }
}
