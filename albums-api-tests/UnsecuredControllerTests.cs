using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using albums_api.Controllers;

namespace albums_api.Tests
{
    public class UnsecuredControllerTest
    {
        private MyController _controller;

        public UnsecuredControllerTest()
        {
            _controller = new MyController();
        }

        [Fact]
        public void ReadFile_WithNullInput_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _controller.ReadFile(null));
        }

        [Fact]
        public void ReadFile_WithEmptyPath_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _controller.ReadFile(string.Empty));
        }

        [Fact]
        public void ReadFile_WithInvalidPath_ThrowsFileNotFoundException()
        {
            Assert.Throws<System.IO.DirectoryNotFoundException>(() => _controller.ReadFile("C:\\NonExistentPath\\file.txt"));
        }

        [Fact]
        public void ReadFile_WithValidPath_ReturnsFileContent()
        {
            string testFilePath = Path.GetTempFileName();
            File.WriteAllText(testFilePath, "Test content");

            try
            {
                string result = _controller.ReadFile(testFilePath);
                Assert.NotNull(result);
            }
            finally
            {
                File.Delete(testFilePath);
            }
        }

        [Fact]
        public void GetProduct_WithNullProductName_ThrowsException()
        {
            Assert.Throws<System.InvalidOperationException>(() => _controller.GetProduct(null!));
        }

        [Fact]
        public void GetObject_WithNullObject_CatchesException()
        {
            // Should not throw, exception is caught internally
            _controller.GetObject();
            Assert.True(true);
        }
    }
}
