using NUnit.Framework;
using System;
using UnityEngine;

namespace Uft.UnityUtils.Tests.Editor
{
    public class TransformUtil_Test
    {
        static (GameObject, GameObject) Arrange_GetDirection()
        {
            var from = new GameObject();
            from.transform.position = new Vector3(1, 2, 3);
            var to = new GameObject();
            to.transform.position = new Vector3(-1, 5, 9);
            return (from, to);
        }

        // GetDirection

        [Test]
        public void GetDirection_from_to_Gets3DNormalizedVector()
        {
            // Arrange
            var (from, to) = Arrange_GetDirection();

            // Act
            var result = TransformUtil.GetDirection(from.transform, to.transform);

            // Assert
            var length = Mathf.Sqrt(2 * 2 + 3 * 3 + 6 * 6);
            Assert.AreEqual((to.transform.position.x - from.transform.position.x) / length, result.x, 0.001);
            Assert.AreEqual((to.transform.position.y - from.transform.position.y) / length, result.y, 0.001);
            Assert.AreEqual((to.transform.position.z - from.transform.position.z) / length, result.z, 0.001);
        }

        // Rotate_

        static (GameObject, GameObject) Arrange_Rotate_()
        {
            var parent = new GameObject();
            parent.transform.position = new Vector3(1, 2, 0);
            var child = new GameObject();
            child.transform.position = parent.transform.position;
            child.transform.SetParent(parent.transform, true);
            child.transform.Translate(-1, 0, 0);
            return (parent, child);
        }

        [Test]
        public void RotateX01_transform_angle_AreOK()
        {
            // parentRoll=(45, Self), childRoll=(30, Self)

            // Arrange
            var (parent, child) = Arrange_Rotate_();

            // Act
            TransformUtil.RotateX(parent.transform, 45);
            TransformUtil.RotateX(child.transform, 30);

            // Quaternion = ( v[x]sin(t/2), v[y]sin(t/2), v[z]sin(t/2), cos(t/2) )

            // Assert
            // parent
            Assert.AreEqual(1, parent.transform.position.x, 0.001);
            Assert.AreEqual(2, parent.transform.position.y, 0.001);
            Assert.AreEqual(0, parent.transform.position.z, 0.001);
            Assert.AreEqual(0.38268343236, parent.transform.rotation.x, 0.001); // v[x]sin(t/2) = sin(45deg / 2) = 0.38268343236
            Assert.AreEqual(0.00000000000, parent.transform.rotation.y, 0.001);
            Assert.AreEqual(0.00000000000, parent.transform.rotation.z, 0.001);
            Assert.AreEqual(0.92387953251, parent.transform.rotation.w, 0.001); // cos(t/2)     = cos(45deg / 2) = 0.92387953251
            // child : rotate 45's x&y => 1/root2
            Assert.AreEqual(1 + (-1), child.transform.position.x, 0.001);
            Assert.AreEqual(2 + (0), child.transform.position.y, 0.001);
            Assert.AreEqual(0 + (0), child.transform.position.z, 0.001);
            Assert.AreEqual(0.60876142900, child.transform.rotation.x, 0.001); // v[x]sin(t/2) = sin(75deg / 2) = 0.60876142900
            Assert.AreEqual(0.00000000000, child.transform.rotation.y, 0.001);
            Assert.AreEqual(0.00000000000, child.transform.rotation.z, 0.001);
            Assert.AreEqual(0.79335334029, child.transform.rotation.w, 0.001); //     cos(t/2) = cos(75deg / 2) = 0.79335334029
        }

        [Test]
        public void RotateX02_transform_angle_World_AreOK()
        {
            // parentRoll=(45, Self), childRoll=(180, World)

            // Arrange
            var (parent, child) = Arrange_Rotate_();

            // Act
            TransformUtil.RotateX(parent.transform, 45);
            TransformUtil.RotateX(child.transform, 180, Space.World); // "Rotate" is same effect for between self and world, because this rotation is z-roll only.

            // Quaternion = ( v[x]sin(t/2), v[y]sin(t/2), v[z]sin(t/2), cos(t/2) )

            // Assert
            // parent
            Assert.AreEqual(1, parent.transform.position.x, 0.001);
            Assert.AreEqual(2, parent.transform.position.y, 0.001);
            Assert.AreEqual(0, parent.transform.position.z, 0.001);
            Assert.AreEqual(0.38268343236, parent.transform.rotation.x, 0.001); // v[x]sin(t/2) = sin(45deg / 2) = 0.38268343236
            Assert.AreEqual(0.00000000000, parent.transform.rotation.y, 0.001);
            Assert.AreEqual(0.00000000000, parent.transform.rotation.z, 0.001);
            Assert.AreEqual(0.92387953251, parent.transform.rotation.w, 0.001); // cos(t/2)     = cos(45deg / 2) = 0.92387953251
            // child : rotate 45's x&y => 1/root2
            Assert.AreEqual(1 + (-1), child.transform.position.x, 0.001);
            Assert.AreEqual(2 + (0), child.transform.position.y, 0.001);
            Assert.AreEqual(0 + (0), child.transform.position.z, 0.001);
            Assert.AreEqual(+0.92387953251, child.transform.rotation.x, 0.001); //  v[y]sin(t/2) = sin(180deg / 2 + 45deg / 2) = +0.92387953251
            Assert.AreEqual(0.00000000000, child.transform.rotation.y, 0.001);
            Assert.AreEqual(0.00000000000, child.transform.rotation.z, 0.001);
            Assert.AreEqual(-0.38268343236, child.transform.rotation.w, 0.001); //     cos(t/2) = cos(180deg / 2 + 45deg / 2) = -0.38268343236
        }

        [Test]
        public void RotateY01_transform_angle_AreOK()
        {
            // parentRoll=(45, Self), childRoll=(30, Self)

            // Arrange
            var (parent, child) = Arrange_Rotate_();

            // Act
            TransformUtil.RotateY(parent.transform, 45);
            TransformUtil.RotateY(child.transform, 30);

            // Quaternion = ( v[x]sin(t/2), v[y]sin(t/2), v[z]sin(t/2), cos(t/2) )

            // Assert
            // parent
            Assert.AreEqual(1, parent.transform.position.x, 0.001);
            Assert.AreEqual(2, parent.transform.position.y, 0.001);
            Assert.AreEqual(0, parent.transform.position.z, 0.001);
            Assert.AreEqual(0.00000000000, parent.transform.rotation.x, 0.001);
            Assert.AreEqual(0.38268343236, parent.transform.rotation.y, 0.001); // v[y]sin(t/2) = sin(45deg / 2) = 0.38268343236
            Assert.AreEqual(0.00000000000, parent.transform.rotation.z, 0.001);
            Assert.AreEqual(0.92387953251, parent.transform.rotation.w, 0.001); // cos(t/2)     = cos(45deg / 2) = 0.92387953251
            // child : rotate 45's x&y => 1/root2
            Assert.AreEqual(1 + (-1 * 1 / Math.Sqrt(2)), child.transform.position.x, 0.001);
            Assert.AreEqual(2, child.transform.position.y, 0.001);
            Assert.AreEqual(0 + (+1 * 1 / Math.Sqrt(2)), child.transform.position.z, 0.001);
            Assert.AreEqual(0.00000000000, child.transform.rotation.x, 0.001);
            Assert.AreEqual(0.60876142900, child.transform.rotation.y, 0.001); // v[y]sin(t/2) = sin(75deg / 2) = 0.60876142900
            Assert.AreEqual(0.00000000000, child.transform.rotation.z, 0.001);
            Assert.AreEqual(0.79335334029, child.transform.rotation.w, 0.001); //     cos(t/2) = cos(75deg / 2) = 0.79335334029
        }

        [Test]
        public void RotateY02_transform_angle_World_AreOK()
        {
            // parentRoll=(45, Self), childRoll=(180, World)

            // Arrange
            var (parent, child) = Arrange_Rotate_();

            // Act
            TransformUtil.RotateY(parent.transform, 45);
            TransformUtil.RotateY(child.transform, 180, Space.World); // "Rotate" is same effect for between self and world, because this rotation is z-roll only.

            // Quaternion = ( v[x]sin(t/2), v[y]sin(t/2), v[z]sin(t/2), cos(t/2) )

            // Assert
            // parent
            Assert.AreEqual(1, parent.transform.position.x, 0.001);
            Assert.AreEqual(2, parent.transform.position.y, 0.001);
            Assert.AreEqual(0, parent.transform.position.z, 0.001);
            Assert.AreEqual(0.00000000000, parent.transform.rotation.x, 0.001);
            Assert.AreEqual(0.38268343236, parent.transform.rotation.y, 0.001); // v[y]sin(t/2) = sin(45deg / 2) = 0.38268343236
            Assert.AreEqual(0.00000000000, parent.transform.rotation.z, 0.001);
            Assert.AreEqual(0.92387953251, parent.transform.rotation.w, 0.001); // cos(t/2)     = cos(45deg / 2) = 0.92387953251
            // child : rotate 45's x&y => 1/root2
            Assert.AreEqual(1 + (-1 * 1 / Math.Sqrt(2)), child.transform.position.x, 0.001);
            Assert.AreEqual(2, child.transform.position.y, 0.001);
            Assert.AreEqual(0 + (+1 * 1 / Math.Sqrt(2)), child.transform.position.z, 0.001);
            Assert.AreEqual(0.00000000000, child.transform.rotation.x, 0.001);
            Assert.AreEqual(+0.92387953251, child.transform.rotation.y, 0.001); //  v[y]sin(t/2) = sin(180deg / 2 + 45deg / 2) = +0.92387953251
            Assert.AreEqual(0.00000000000, child.transform.rotation.z, 0.001);
            Assert.AreEqual(-0.38268343236, child.transform.rotation.w, 0.001); //     cos(t/2) = cos(180deg / 2 + 45deg / 2) = -0.38268343236
        }

        [Test]
        public void RotateZ01_transform_angle_AreOK()
        {
            // parentRoll=(45, Self), childRoll=(30, Self)

            // Arrange
            var (parent, child) = Arrange_Rotate_();

            // Act
            TransformUtil.RotateZ(parent.transform, 45);
            TransformUtil.RotateZ(child.transform, 30);

            // Quaternion = ( v[x]sin(t/2), v[y]sin(t/2), v[z]sin(t/2), cos(t/2) )

            // Assert
            // parent
            Assert.AreEqual(1, parent.transform.position.x, 0.001);
            Assert.AreEqual(2, parent.transform.position.y, 0.001);
            Assert.AreEqual(0, parent.transform.position.z, 0.001);
            Assert.AreEqual(0.00000000000, parent.transform.rotation.x, 0.001);
            Assert.AreEqual(0.00000000000, parent.transform.rotation.y, 0.001);
            Assert.AreEqual(0.38268343236, parent.transform.rotation.z, 0.001); // v[z]sin(t/2) = sin(45deg / 2) = 0.38268343236
            Assert.AreEqual(0.92387953251, parent.transform.rotation.w, 0.001); // cos(t/2)     = cos(45deg / 2) = 0.92387953251
            // child : rotate 45's x&y => 1/root2
            Assert.AreEqual(1 + (-1 * 1 / Math.Sqrt(2)), child.transform.position.x, 0.001);
            Assert.AreEqual(2 + (-1 * 1 / Math.Sqrt(2)), child.transform.position.y, 0.001);
            Assert.AreEqual(0, child.transform.position.z, 0.001);
            Assert.AreEqual(0.00000000000, child.transform.rotation.x, 0.001);
            Assert.AreEqual(0.00000000000, child.transform.rotation.y, 0.001);
            Assert.AreEqual(0.60876142900, child.transform.rotation.z, 0.001); // v[z]sin(t/2) = sin(75deg / 2) = 0.60876142900
            Assert.AreEqual(0.79335334029, child.transform.rotation.w, 0.001); //     cos(t/2) = cos(75deg / 2) = 0.79335334029
        }

        [Test]
        public void RotateZ02_transform_angle_World_AreOK()
        {
            // parentRoll=(45, Self), childRoll=(180, World)

            // Arrange
            var (parent, child) = Arrange_Rotate_();

            // Act
            TransformUtil.RotateZ(parent.transform, 45);
            TransformUtil.RotateZ(child.transform, 180, Space.World); // "Rotate" is same effect for between self and world, because this rotation is z-roll only.

            // Quaternion = ( v[x]sin(t/2), v[y]sin(t/2), v[z]sin(t/2), cos(t/2) )

            // Assert
            // parent
            Assert.AreEqual(1, parent.transform.position.x, 0.001);
            Assert.AreEqual(2, parent.transform.position.y, 0.001);
            Assert.AreEqual(0, parent.transform.position.z, 0.001);
            Assert.AreEqual(0.00000000000, parent.transform.rotation.x, 0.001);
            Assert.AreEqual(0.00000000000, parent.transform.rotation.y, 0.001);
            Assert.AreEqual(0.38268343236, parent.transform.rotation.z, 0.001); // v[z]sin(t/2) = sin(45deg / 2) = 0.38268343236
            Assert.AreEqual(0.92387953251, parent.transform.rotation.w, 0.001); // cos(t/2)     = cos(45deg / 2) = 0.92387953251
            // child : rotate 45's x&y => 1/root2
            Assert.AreEqual(1 + (-1 * 1 / Math.Sqrt(2)), child.transform.position.x, 0.001);
            Assert.AreEqual(2 + (-1 * 1 / Math.Sqrt(2)), child.transform.position.y, 0.001);
            Assert.AreEqual(0, child.transform.position.z, 0.001);
            Assert.AreEqual(0.00000000000, child.transform.rotation.x, 0.001);
            Assert.AreEqual(0.00000000000, child.transform.rotation.y, 0.001);
            Assert.AreEqual(+0.92387953251, child.transform.rotation.z, 0.001); // v[z]sin(t/2) = sin(180deg / 2 + 45deg / 2) = +0.92387953251
            Assert.AreEqual(-0.38268343236, child.transform.rotation.w, 0.001); //     cos(t/2) = cos(180deg / 2 + 45deg / 2) = -0.38268343236
        }

        // Translate_

        static (GameObject, GameObject) Arrange_Translate_()
        {
            var parent = new GameObject();
            parent.transform.position = new Vector3(1, 2, 0);
            var child = new GameObject();
            child.transform.position = parent.transform.position;
            child.transform.SetParent(parent.transform, true);
            child.transform.Translate(-1, 0, 0);
            return (parent, child);
        }

        [Test]
        public void TranslateX01_transform_x_AreOK()
        {
            // Arrange
            var (parent, child) = Arrange_Translate_();

            // Act
            TransformUtil.TranslateX(parent.transform, 10);
            parent.transform.Rotate(new Vector3(0, 0, 1), 45);
            TransformUtil.TranslateX(child.transform, 10);

            // Assert
            Assert.AreEqual(1 + 10, parent.transform.position.x, 0.001);
            Assert.AreEqual(2, parent.transform.position.y, 0.001);
            Assert.AreEqual(0, parent.transform.position.z, 0.001);
            Assert.AreEqual(1 + 10 + ((-1 + 10) / Math.Sqrt(2)), child.transform.position.x, 0.001);
            Assert.AreEqual(2 + 0 + ((-1 + 10) / Math.Sqrt(2)), child.transform.position.y, 0.001);
            Assert.AreEqual(0, child.transform.position.z, 0.001);
        }

        [Test]
        public void TranslateX02_transform_x_World_AreOK()
        {
            // Arrange
            var (parent, child) = Arrange_Translate_();

            // Act
            TransformUtil.TranslateX(parent.transform, 10);
            parent.transform.Rotate(new Vector3(0, 0, 1), 45);
            TransformUtil.TranslateX(child.transform, 50, Space.World);

            // Assert
            Assert.AreEqual(1 + 10, parent.transform.position.x, 0.001);
            Assert.AreEqual(2, parent.transform.position.y, 0.001);
            Assert.AreEqual(0, parent.transform.position.z, 0.001);
            Assert.AreEqual(1 + 10 - (1 / Math.Sqrt(2)) + 50, child.transform.position.x, 0.001);
            Assert.AreEqual(2 + 0 - (1 / Math.Sqrt(2)), child.transform.position.y, 0.001);
            Assert.AreEqual(0, child.transform.position.z, 0.001);
        }

        [Test]
        public void TranslateY01_transform_y_AreOK()
        {
            // Arrange
            var (parent, child) = Arrange_Translate_();

            // Act
            TransformUtil.TranslateY(parent.transform, 10);
            parent.transform.Rotate(new Vector3(0, 0, 1), 45);
            TransformUtil.TranslateY(child.transform, 10);

            // Assert
            Assert.AreEqual(1, parent.transform.position.x, 0.001);
            Assert.AreEqual(2 + 10, parent.transform.position.y, 0.001);
            Assert.AreEqual(0, parent.transform.position.z, 0.001);
            Assert.AreEqual(1 + 0 - ((1 + 10) / Math.Sqrt(2)), child.transform.position.x, 0.001);
            Assert.AreEqual(2 + 10 + ((-1 + 10) / Math.Sqrt(2)), child.transform.position.y, 0.001);
            Assert.AreEqual(0, child.transform.position.z, 0.001);
        }

        [Test]
        public void TranslateY02_transform_y_World_AreOK()
        {
            // Arrange
            var (parent, child) = Arrange_Translate_();

            // Act
            TransformUtil.TranslateY(parent.transform, 10);
            parent.transform.Rotate(new Vector3(0, 0, 1), 45);
            TransformUtil.TranslateY(child.transform, 50, Space.World);

            // Assert
            Assert.AreEqual(1, parent.transform.position.x, 0.001);
            Assert.AreEqual(2 + 10, parent.transform.position.y, 0.001);
            Assert.AreEqual(0, parent.transform.position.z, 0.001);
            Assert.AreEqual(1 + 0 - (1 / Math.Sqrt(2)), child.transform.position.x, 0.001);
            Assert.AreEqual(2 + 10 - (1 / Math.Sqrt(2)) + 50, child.transform.position.y, 0.001);
            Assert.AreEqual(0, child.transform.position.z, 0.001);
        }

        [Test]
        public void TranslateZ01_transform_z_AreOK()
        {
            // Arrange
            var (parent, child) = Arrange_Translate_();

            // Act
            TransformUtil.TranslateZ(parent.transform, 10);
            parent.transform.Rotate(new Vector3(0, 0, 1), 45);
            TransformUtil.TranslateZ(child.transform, 10);

            // Assert
            Assert.AreEqual(1, parent.transform.position.x, 0.001);
            Assert.AreEqual(2, parent.transform.position.y, 0.001);
            Assert.AreEqual(0 + 10, parent.transform.position.z, 0.001);
            Assert.AreEqual(1 - (1 / Math.Sqrt(2)), child.transform.position.x, 0.001);
            Assert.AreEqual(2 - (1 / Math.Sqrt(2)), child.transform.position.y, 0.001);
            Assert.AreEqual(0 + 10 + 10, child.transform.position.z, 0.001);
        }

        [Test]
        public void TranslateZ02_transform_z_World_AreOK()
        {
            // Arrange
            var (parent, child) = Arrange_Translate_();

            // Act
            TransformUtil.TranslateZ(parent.transform, 10);
            parent.transform.Rotate(new Vector3(0, 0, 1), 45);
            TransformUtil.TranslateZ(child.transform, 50, Space.World);

            // Assert
            Assert.AreEqual(1, parent.transform.position.x, 0.001);
            Assert.AreEqual(2, parent.transform.position.y, 0.001);
            Assert.AreEqual(0 + 10, parent.transform.position.z, 0.001);
            Assert.AreEqual(1 - (1 / Math.Sqrt(2)), child.transform.position.x, 0.001);
            Assert.AreEqual(2 - (1 / Math.Sqrt(2)), child.transform.position.y, 0.001);
            Assert.AreEqual(0 + 10 + 50, child.transform.position.z, 0.001);
        }
    }
}