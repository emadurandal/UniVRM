﻿using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using ColorSpace = UniGLTF.ColorSpace;

namespace VRMShaders
{
    public class TextureBytesTests
    {
        static string AssetPath = "Assets/VRMShaders/GLTF/IO/Tests";

        [Test]
        public void NonReadablePng()
        {
            var nonReadableTex = AssetDatabase.LoadAssetAtPath<Texture2D>($"{AssetPath}/4x4.png");
            Assert.False(nonReadableTex.isReadable);
            var (bytes, mime) = AssetTextureUtil.GetTextureBytesWithMime(nonReadableTex, ColorSpace.sRGB);
            Assert.NotNull(bytes);
        }

        [Test]
        public void NonReadableDds()
        {
            var readonlyTexture = AssetDatabase.LoadAssetAtPath<Texture2D>($"{AssetPath}/4x4compressed.dds");
            Assert.False(readonlyTexture.isReadable);
            var (bytes, mime) = AssetTextureUtil.GetTextureBytesWithMime(readonlyTexture, ColorSpace.sRGB);
            Assert.NotNull(bytes);
        }
    }
}