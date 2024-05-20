using NUnit.Framework;
using QLMonAn; // Đảm bảo rằng namespace này đúng với dự án của bạn
using System.Data.SqlClient;

namespace QLMonAn.Tests
{
    [TestFixture]
    public class AdminFormTests
    {
        private AdminForm _adminForm;

        [SetUp]
        public void Setup()
        {
            _adminForm = new AdminForm();
        }

        [Test]
        public void TestInitializeDatabaseConnection_Success()
        {
            // Arrange
            string connectionString = "Data Source=LAPTOP-R6GO7FIS\\CHUKINT;Initial Catalog=QuanLyMonAn;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);

            // Act
            connection.Open();

            // Assert
            Assert.That(connection.State, Is.EqualTo(System.Data.ConnectionState.Open));

            // Clean up
            connection.Close();
        }

        [Test]
        public void TestAddFood_Success()
        {
            // Arrange
            string foodName = "Phở";
            string unit = "Bát";
            decimal price = 30000;
            string group = "Món nước";

            // Act
            bool result = _adminForm.AddFood(foodName, unit, price, group);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void TestSearchFood_Found()
        {
            // Arrange
            string searchQuery = "Phở";

            // Act
            var result = _adminForm.(searchQuery);

            // Assert
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void TestSearchFood_NotFound()
        {
            // Arrange
            string searchQuery = "Bánh xèo";

            // Act
            var result = _adminForm.SearchFood(searchQuery);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up code if needed
        }
    }
}
