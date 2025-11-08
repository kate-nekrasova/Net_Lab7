using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net_Lab7;

namespace laba7_test
{
    [TestClass]
    public class FileHandler
    {
        [TestClass]
        public class FileHandlerTest
        {
            private string _testFilePath;

            [TestInitialize]
            public void TestInitialize()
            {
                _testFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".json");
            }

            [TestCleanup]
            public void Cleanup()
            {
                if (File.Exists(_testFilePath))
                {
                    File.Delete(_testFilePath);
                }
            }


            [TestMethod]
            public async Task SaveAsync_Should_CreateFile()
            {
                var students = new List<Student>
                {
                    new Student("Hanna Washington", "Lyceum 39", 2000), 
                    new Student("Beth Washington", "Lyceum 39", 2001)
                };

                var fileSaver = new JsonFileWriter();
                await fileSaver.SaveAsync(_testFilePath, students);

                Assert.IsTrue(File.Exists(_testFilePath), "Файл не було створено");
            }

            [TestMethod]
            public async Task SaveAsync_Should_WriteNonEmptyContent()
            {
                var students = new List<Student>
    {
        new Student("Hanna Washington", "Lyceum 39", 2000)
    };

                var fileSaver = new JsonFileWriter();
                await fileSaver.SaveAsync(_testFilePath, students);

                string content = await File.ReadAllTextAsync(_testFilePath);
                Assert.IsFalse(string.IsNullOrEmpty(content), "Файл порожній");
            }


            [TestMethod]
            public async Task SaveAndLoadAsync_Should_PreserveStudentData()
            {
                var students = new List<Student>
    {
        new Student("Hanna Washington", "Lyceum 39", 2000)
    };

                var fileSaver = new JsonFileWriter();
                await fileSaver.SaveAsync(_testFilePath, students);

                var fileReader = new JsonFileReader();
                var loadedStudents = await fileReader.LoadAsync<Student>(_testFilePath);

                Assert.IsNotNull(loadedStudents, "Десеріалізація не вдалася");
                Assert.AreEqual(1, loadedStudents.Count, "К-сть десеріалізованих об'єктів не збігається");
                Assert.AreEqual("Hanna Washington", loadedStudents[0].Name);
                Assert.AreEqual("Lyceum 39", loadedStudents[0].School);
            }


            //load async
            [TestMethod]
            public async Task LoadAsync_Should_NotReturnNull()
            {
                var jsonContent = "[{\"Name\":\"Hanna Washington\",\"School\":\"Lyceum 39\",\"Year\":2000}]";
                await File.WriteAllTextAsync(_testFilePath, jsonContent);
                var fileReader = new JsonFileReader();

                var loadedStudents = await fileReader.LoadAsync<Student>(_testFilePath);

                Assert.IsNotNull(loadedStudents, "Об'єкти не були завантажені");
            }

            [TestMethod]
            public async Task LoadAsync_Should_ReturnCorrectCount()
            {
                var jsonContent = "[{\"Name\":\"Hanna Washington\",\"School\":\"Lyceum 39\",\"Year\":2000}]";
                await File.WriteAllTextAsync(_testFilePath, jsonContent);
                var fileReader = new JsonFileReader();

                var loadedStudents = await fileReader.LoadAsync<Student>(_testFilePath);

                Assert.AreEqual(1, loadedStudents?.Count, "Завантажена некоректна к-сть об'єктів");
            }

            [TestMethod]
            public async Task LoadAsync_Should_ReturnCorrectStudentData()
            {
                var jsonContent = "[{\"Name\":\"Hanna Washington\",\"School\":\"Lyceum 39\",\"Year\":2000}]";
                await File.WriteAllTextAsync(_testFilePath, jsonContent);
                var fileReader = new JsonFileReader();

                var loadedStudents = await fileReader.LoadAsync<Student>(_testFilePath);
                var student = loadedStudents[0];

                Assert.AreEqual("Hanna Washington", student.Name);
                Assert.AreEqual("Lyceum 39", student.School);
                Assert.AreEqual(2000, student.Year);
            }


            [TestMethod]
            public async Task LoadAsync_WhenFileDoesntExist_ReturnsNull()
            {
                var fileReader = new JsonFileReader();
                var loadedStudents = await fileReader.LoadAsync<Student>(_testFilePath);
                Assert.IsNull(loadedStudents, "Повинен повертаити null, якщо фай не існує");
            }
        }
    }
}