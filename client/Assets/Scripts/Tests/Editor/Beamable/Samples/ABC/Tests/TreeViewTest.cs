using System;
using System.Collections;
using Beamable.Samples.ABC.Views;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Beamable.Samples.ABC
{
    public class TreeViewTest
    {
        private GameObject _parentGameObject = null;

        /// <summary>
        /// Before each test, instantiate the related prefab instance
        /// </summary>
        [SetUp]
        public void OnSetup()
        {
            if (_parentGameObject != null)
            {
                throw new Exception("_parentGameObject must be null here.");
            }

            TreeView treeViewPrefab = AssetDatabase.LoadAssetAtPath<TreeView>("Assets/Prefabs/TreeView.prefab");
            TreeView treeViewInstance = GameObject.Instantiate<TreeView>(treeViewPrefab);
            _parentGameObject = treeViewInstance.gameObject;
        }
        
        /// <summary>
        /// After each test, destroy the related prefab instance
        /// </summary>
        [TearDown]
        public void OnTearDown()
        {
            if (_parentGameObject == null)
            {
                return;
            }
            
            GameObject.DestroyImmediate(_parentGameObject, false);
        }

        [UnityTest]
        public IEnumerator GrowthPercentage_ResultIs0_WhenDefault()
        {
            // Arrange
            TreeView treeView = _parentGameObject.GetComponent<TreeView>();
            
            // A Delay
            yield return null;
            
            // Act
            
            // Assert
            Assert.That(treeView.GrowthPercentage, Is.EqualTo(0));
        }
        
        [UnityTest]
        public IEnumerator GrowthPercentage_ResultIs10_WhenValueIs10()
        {
            // Arrange
            float value = 10;
            TreeView treeView = _parentGameObject.GetComponent<TreeView>();
            
            // A Delay
            yield return null;
            
            // Act
            treeView.GrowthPercentage = value;
            
            // Assert
            Assert.That(treeView.GrowthPercentage, Is.EqualTo(10));
        }
    }
}
