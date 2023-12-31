﻿using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category
{
    [Collection(nameof(CategoryTestFixture))]
    public class CategoryTest
    {
        private readonly CategoryTestFixture _categoryTestFixture;

        public CategoryTest(CategoryTestFixture categoryTestFixture)
            => _categoryTestFixture = categoryTestFixture;

        [Fact(DisplayName = nameof(Instantiate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Instantiate()
        {
            // Arrange
            var validData = _categoryTestFixture.GetValidCategory();

            // Act
            var category = new DomainEntity.Category(validData.Name, validData.Description);

            // Assert
            category.Should().NotBeNull();
            category.Name.Should().Be(validData.Name);
            category.Description.Should().Be(validData.Description);
            category.Id.Should().NotBeEmpty();
            category.CreatedAt.Should().NotBeSameDateAs(default(DateTime));
            category.IsActive.Should().BeTrue();
        }

        [Theory(DisplayName = nameof(InstantiateWithIsActive))]
        [Trait("Domain", "Category - Aggregates")]
        [InlineData(true)]
        [InlineData(false)]
        public void InstantiateWithIsActive(bool isActive)
        {
            // Arrange
            var validData = _categoryTestFixture.GetValidCategory();

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
            var validCategory = _categoryTestFixture.GetValidCategory();

            Action action = () => new DomainEntity.Category(name!, validCategory.Description);

            // Act & Assert
            action.Should().Throw<EntityValidationException>()
                           .WithMessage("Name should not be null or empty");
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsEmpty))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsEmpty()
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();

            Action action = () => new DomainEntity.Category(validCategory.Name, null!);

            // Act
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Description should not be null or empty", exception.Message);
        }

        [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
        [Trait("Domain", "Category - Aggregates")]
        [MemberData(nameof(GetNamesWithLessThan3Characters), parameters: 6)]
        public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
        {
            // Arrange
            var validCategory = _categoryTestFixture.GetValidCategory();

            Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);

            // Act & Assert
            action.Should().Throw<EntityValidationException>()
                           .WithMessage("Name should not be less than 3 characters long");
        }

        public static IEnumerable<object[]> GetNamesWithLessThan3Characters(int numberOfInteractions)
        {
            var fixture = new CategoryTestFixture();

            for (int i = 0; i < numberOfInteractions; i++)
            {
                var isOdd = i % 2 == 1;
                yield return new object[] { fixture.GetValidCategoryName().Substring(0, isOdd ? 1 : 2) };
            }
        }

        [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenNameIsGreaterThan255Characters()
        {
            // Arrange
            var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

            var validCategory = _categoryTestFixture.GetValidCategory();

            Action action = () => new DomainEntity.Category(invalidName, validCategory.Description);

            // Act
            var exception = Assert.Throws<EntityValidationException>(action);

            // Assert
            Assert.Equal("Name should not be greater than 255 characters long", exception.Message);
        }


        [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
        [Trait("Domain", "Category - Aggregates")]
        public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
        {
            // Arrange
            var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());

            var validCategory = _categoryTestFixture.GetValidCategory();

            Action action = () => new DomainEntity.Category(validCategory.Name, invalidDescription);

            // Act & Assert
            action.Should().Throw<EntityValidationException>()
                           .WithMessage("Description should not be greater than 10000 characters long");
        }

        [Fact(DisplayName = nameof(Activate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Activate()
        {
            // Arrange
            var validData = _categoryTestFixture.GetValidCategory();

            // Act
            var category = new DomainEntity.Category(validData.Name, validData.Description, true);
            category.Activate();

            // Assert
            Assert.True(category.IsActive);
        }

        [Fact(DisplayName = nameof(Deactivate))]
        [Trait("Domain", "Category - Aggregates")]
        public void Deactivate()
        {
            // Arrange
            var validData = _categoryTestFixture.GetValidCategory();

            // Act
            var category = new DomainEntity.Category(validData.Name, validData.Description, true);
            category.Deactivate();

            // Assert
            category.IsActive.Should().BeFalse();
        }

        [Fact(DisplayName = nameof(Update))]
        [Trait("Domain", "Category - Aggregates")]
        public void Update()
        {
            // Arrange
            var category = _categoryTestFixture.GetValidCategory();

            var categoryWithNewValues = _categoryTestFixture.GetValidCategory();

            // Act
            category.Update(categoryWithNewValues.Name, categoryWithNewValues.Description);

            // Assert
            category.Name.Should().Be(categoryWithNewValues.Name);
            category.Description.Should().Be(categoryWithNewValues.Description);
        }

        [Fact(DisplayName = nameof(UpdateOnlyName))]
        [Trait("Domain", "Category - Aggregates")]
        public void UpdateOnlyName()
        {
            // Arrange
            var category = _categoryTestFixture.GetValidCategory();

            var newName = _categoryTestFixture.GetValidCategoryName();
            var currentDescription = category.Description;

            // Act
            category.Update(newName);

            // Assert
            Assert.Equal(newName, category.Name);
            Assert.Equal(currentDescription, category.Description);
        }
    }
}
