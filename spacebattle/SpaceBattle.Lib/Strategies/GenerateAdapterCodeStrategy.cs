using Scriban;

namespace SpaceBattle.Lib;

public class GenerateAdapterCodeStrategy : IStrategy
{
    public object Run(object[] args)
    {
        var type = (Type)args[0];

        var template = Template.Parse(@"
            public class {{ type.name }}Adapter : {{ type.name }}
            {
                private readonly IUObject _obj;
                public {{type.name}}Adapter(IUObject obj)
                {
                    _obj = obj;
                }{{for property in properties}}
                public {{property.property_type.name}} {{property.name}}
                {
                    {{if property.can_read}}get => _obj.GetProperty(""{{property.name}}"");{{end}}{{if property.can_write}}
                    set => _obj.SetProperty(""{{property.name}}"", value);{{end}}
                }{{end}}
            }"
        );
        
        var result = template.Render(new { type, properties = type.GetProperties() });

        return result;
    }
}