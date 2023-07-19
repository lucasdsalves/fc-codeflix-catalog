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

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData("a")]
        [InlineData("ab")]
        public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            // Arrange
            Action action = () => new DomainEntity.Category(invalidName, "category description");

            // Act
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Name should be at least 3 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenNameIsGreaterThan255Characters()
        {
            // Arrange
            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

            Action action = () => new DomainEntity.Category(invalidName, "category description");

            // Act
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Name should be less or equal to 255 characters long", exception.Message);
        }


        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
        {
            // Arrange
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());

            Action action = () => new DomainEntity.Category("category name", invalidDescription);

            // Act
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Description should be less or equal to 10000 characters long", exception.Message);
        }

        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Deactivate()
        {
            // Arrange
            var validData = new
            {
                Name = "category name",
                Description = "category description"
            };

            // Act
            var category = new DomainEntity.Category(validData.Name, validData.Description, true);
            category.Deactivate();

            // Assert
            Assert.False(category.IsActive);
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update()
        {
            // Arrange
            var category = new DomainEntity.Category("category name", "category description");
            var newValues = new { Name = "new name", Description = "new description" };

            // Act
            category.Update(newValues.Name, newValues.Description);

            // Assert
            Assert.Equal(newValues.Name, category.Name);
            Assert.Equal(newValues.Description, category.Description);
        }

        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyName()
        {
            // Arrange
            var category = new DomainEntity.Category("category name", "category description");
            var newValues = new { Name = "new name" };
            var currentDescription = category.Description;

            // Act
            category.Update(newValues.Name);

            // Assert
            Assert.Equal(newValues.Name, category.Name);
            Assert.Equal(currentDescription, category.Description);
        }
    }
}
