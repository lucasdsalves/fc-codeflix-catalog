
namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation
{
    public class DomainValidationTest
    {
        private Faker Faker { get; set; } = new Faker();

        [Fact(DisplayName = nameof(NotNullOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOk()
        {
            var value = Faker.Commerce.ProductName();
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action =
                () => DomainValidation.NotNull(value, fieldName);

            action.Should().NotThrow();
        }

        [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullThrowWhenNull()
        {
            string? value = null;
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action =
                () => DomainValidation.NotNull(value, fieldName);

            action.Should()
                  .Throw<EntityValidationException>()
                  .WithMessage($"{fieldName} should not be null");
        }

        [Fact(DisplayName = (nameof(NotNullOrEmptyOk)))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOrEmptyOk()
        {
            var target = Faker.Commerce.ProductName();
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action =
                () => DomainValidation.NotNullOrEmpty(target, fieldName);

            action.Should()
                  .NotThrow();
        }

        [Theory(DisplayName = (nameof(NotNullOrEmptyThrowWhenEmpty)))]
        [Trait("Domain", "DomainValidation - Validation")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void NotNullOrEmptyThrowWhenEmpty(string? target)
        {
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action =
                () => DomainValidation.NotNullOrEmpty(target, fieldName);

            action.Should()
                  .Throw<EntityValidationException>()
                  .WithMessage($"{fieldName} should not be null or empty");
        }

        [Theory(DisplayName = (nameof(MinLengthOk)))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesGreaterThanMin), parameters: 3)]
        public void MinLengthOk(string target, int minLength)
        {
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action =
                () => DomainValidation.MinLength(target, minLength, fieldName);

            action.Should()
                  .NotThrow();
        }

        public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberOfInteractions)
        {
            var faker = new Faker();

            for (int i = 0; i < numberOfInteractions; i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length - (new Random()).Next(1, 5);

                yield return new object[] { example, minLength };
            }
        }

        [Theory(DisplayName = (nameof(MinLengthThrowWhenLess)))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesSmallerThanMin), parameters: 3)]
        public void MinLengthThrowWhenLess(string target, int minLength)
        {
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action =
                () => DomainValidation.MinLength(target, minLength, fieldName);

            action.Should()
                  .Throw<EntityValidationException>()
                  .WithMessage($"{fieldName} should not be less than {minLength} characters long");
        }

        public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberOfInteractions)
        {
            var faker = new Faker();

            for (int i = 0; i < numberOfInteractions; i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length + (new Random()).Next(1, 5);

                yield return new object[] { example, minLength };
            }
        }

        [Theory(DisplayName = (nameof(MaxLengthThrowWhenGreater)))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesGreaterThanMax), parameters: 3)]
        public void MaxLengthThrowWhenGreater(string target, int maxLength)
        {
            string fieldName = Faker.Commerce.ProductName().Replace(" ", "");

            Action action =
                () => DomainValidation.MaxLength(target, maxLength, fieldName);

            action.Should()
                  .Throw<EntityValidationException>()
                  .WithMessage($"{fieldName} should not be greater than {maxLength} characters long");
        }

        public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberOfInteractions)
        {
            var faker = new Faker();

            for (int i = 0; i < numberOfInteractions; i++)
            {
                var example = faker.Commerce.ProductName();
                var maxLength = example.Length - (new Random()).Next(1, 5);

                yield return new object[] { example, maxLength };
            }
        }

        [Theory(DisplayName = (nameof(MaxLengthOk)))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesLessThanMax), parameters: 3)]
        public void MaxLengthOk(string target, int maxLength)
        {
            Action action =
                () => DomainValidation.MaxLength(target, maxLength, "FieldName");

            action.Should()
                  .NotThrow();
        }

        public static IEnumerable<object[]> GetValuesLessThanMax(int numberOfInteractions)
        {
            var faker = new Faker();

            for (int i = 0; i < numberOfInteractions; i++)
            {
                var example = faker.Commerce.ProductName();
                var maxLength = example.Length + (new Random()).Next(0, 5);

                yield return new object[] { example, maxLength };
            }
        }
    }
}
