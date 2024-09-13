namespace BBT.Resource.PolicyEngine.Evaluations.Options;

public class EvaluationConfigurationDictionary : Dictionary<string, EvaluationStepConfiguration>
{
    public void Add(EvaluationStepConfiguration stepConfiguration)
    {
        this[stepConfiguration.Name] = stepConfiguration;
    }
}
