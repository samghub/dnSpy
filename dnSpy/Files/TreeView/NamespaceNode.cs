﻿/*
    Copyright (C) 2014-2015 de4dot@gmail.com

    This file is part of dnSpy

    dnSpy is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    dnSpy is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with dnSpy.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnSpy.Contracts.Files.TreeView;
using dnSpy.Contracts.Highlighting;
using dnSpy.Contracts.Images;
using dnSpy.Contracts.Languages;
using dnSpy.Contracts.TreeView;
using dnSpy.Shared.UI.Files.TreeView;

namespace dnSpy.Files.TreeView {
	sealed class NamespaceNode : FileTreeNodeData, INamespaceNode {
		public override Guid Guid {
			get { return new Guid(FileTVConstants.DNSPY_NAMESPACE_NODE_GUID); }
		}

		protected override ImageReference GetIcon(IDotNetImageManager dnImgMgr) {
			return dnImgMgr.GetNamespaceImageReference();
		}

		public string Name {
			get { return name; }
		}
		readonly string name;

		public override NodePathName NodePathName {
			get { return new NodePathName(Guid, Name); }
		}

		public override ITreeNodeGroup TreeNodeGroup {
			get { return treeNodeGroup; }
		}
		readonly ITreeNodeGroup treeNodeGroup;

		public NamespaceNode(ITreeNodeGroup treeNodeGroup, string name, List<TypeDef> types) {
			this.treeNodeGroup = treeNodeGroup;
			this.name = name;
			this.typesToCreate = types;
		}

		public override void Initialize() {
			TreeNode.LazyLoading = true;
		}

		public override IEnumerable<ITreeNodeData> CreateChildren() {
			foreach (var type in typesToCreate)
				yield return new TypeNode(TreeNodeGroups.TypeTreeNodeGroupNamespace, type);
			typesToCreate = null;
		}
		List<TypeDef> typesToCreate;

		protected override void Write(ISyntaxHighlightOutput output, ILanguage language) {
			new NodePrinter().WriteNamespace(output, language, name);
		}

		public ITypeNode Create(TypeDef type) {
			return Context.FileTreeView.Create(type);
		}
	}
}
