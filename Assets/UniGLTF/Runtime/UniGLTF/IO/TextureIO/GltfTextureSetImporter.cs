﻿using System;
using System.Collections.Generic;
using System.Linq;
using VRMShaders;


namespace UniGLTF
{
    /// <summary>
    /// Texture 生成に関して
    /// Runtimeは LoadImage するだけだが、Editor時には Asset 化するにあたって続きの処理がある。
    ///
    /// * (gltf/glb/vrm-1): AssetImporterContext.AddObjectToAsset(SubAsset)
    /// * (gltf/glb/vrm-1): ScriptedImporter.GetExternalObjectMap(Extracted)
    /// * (vrm-0): (Extracted) ScriptedImporter では無いので ScriptedImporter.AddRemap, GetExternalObjectMap が無い
    ///
    /// AddRemap, GetExternalObjectMap は Dictionary[SourceAssetIdentifier, UnityEngine.Object] に対する API で
    /// SourceAssetIdentifier 型がリソースを識別するキーとなる。
    ///
    /// gltfTexture から SourceAssetIdentifier を作り出すことで、GetExternalObjectMap との対応関係を作る。
    ///
    /// [例外]
    /// glTF で外部ファイルを uri 参照する場合
    /// * sRGB 外部ファイルをそのまま使うので SubAsset にしない
    /// * normal 外部ライルをそのまま使うので SubAsset にしない(normalとしてロードするためにAssetImporterの設定は必用)
    /// * metallicRoughnessOcclusion 変換結果を SubAsset 化する
    /// </summary>
    public sealed class GltfTextureSetImporter : ITextureSetImporter
    {
        private readonly GltfParser m_parser;

        public GltfTextureSetImporter(GltfParser parser)
        {
            m_parser = parser;
        }

        public IEnumerable<TextureImportParam> GetTextureParamsDistinct()
        {
            var usedTextures = new HashSet<SubAssetKey>();
            foreach (var (key, param) in EnumerateAllTextures(m_parser))
            {
                if (usedTextures.Add(key))
                {
                    yield return param;
                }
            }
        }

        /// <summary>
        /// glTF 全体で使うテクスチャーを列挙。
        /// </summary>
        private static IEnumerable<(SubAssetKey, TextureImportParam)> EnumerateAllTextures(GltfParser parser)
        {
            for (int i = 0; i < parser.GLTF.materials.Count; ++i)
            {
                foreach (var kv in GltfPbrTextureImporter.EnumerateTexturesReferencedByMaterial(parser, i))
                {
                    yield return kv;
                }
            }
        }
    }
}