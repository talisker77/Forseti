using Forseti.Harnesses;
using Machine.Specifications;

namespace Forseti.Specs.Harnesses.for_Harness
{
    public class when_checking_if_file_is_description_file_and_it_should_match
	{
		static Harness	harness;
        static bool result;
		
		Establish context = () => 
		{
            harness = new Harness
            {
                DescriptionsSearchPath = "Scripts/{system}.js"
            };
		};

        Because of = () => result = harness.IsDescription("Scripts/something.js");

        It should_acknowledge_file_as_a_description_file = () => result.ShouldBeTrue();
	}
}

