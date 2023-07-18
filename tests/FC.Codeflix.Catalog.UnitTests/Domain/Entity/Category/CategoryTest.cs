using FC.Codeflix.Catalog.Domain.Exceptions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category
{
    public class CategoryTest
    {
        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            // Arrange
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };

            // Act
            var category = new DomainEntity.Category(validData.Name, validData.Description);

            // Assert
            Assert.NotNull(category);
            Assert.Equal(validData.Name, validData.Name);
            Assert.Equal(validData.Description, validData.Description);
            Assert.NotEqual(Guid.Empty, category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.True(category.IsActive);
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActive(bool isActive)
        {
            // Arrange
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };

            // Act
            var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);

            // Assert
            Assert.NotNull(category);
            Assert.Equal(validData.Name, validData.Name);
            Assert.Equal(validData.Description, validData.Description);
            Assert.NotEqual(Guid.Empty, category.Id);
            Assert.NotEqual(default(DateTime), category.CreatedAt);
            Assert.Equal(isActive, category.IsActive);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData(" ")]
        public void InstantiateErrorWhenNameIsEmpty(string? name)
        {
            // Arrange
            Action action = () => new DomainEntity.Category(name!, "category description");

            // Act
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Name should not be empty or null", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsEmpty()
        {
            // Arrange
            Action action = () => new DomainEntity.Category("category name", null!);

            // Act
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Description should not be empty or null", exception.Message);
        }
    }
}
