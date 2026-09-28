using System.Collections.Generic;
using System.Linq;
using ShallowWater.Game.Woods;

namespace ShallowWater.Unity.Woods
{
    public sealed class TreeModels
    {
        private readonly List<Dictionary<DetailLevel, TreeModel>> byForm;

        private TreeModels(List<Dictionary<DetailLevel, TreeModel>> byForm)
        {
            this.byForm = byForm;
        }

        public static TreeModels Made()
        {
            var byForm = TreeForms.All.Select(ModelsOf).ToList();
            return new TreeModels(byForm);
        }

        public TreeModel For(int form, DetailLevel level)
        {
            var isFarAway = level == DetailLevel.Far;
            var shown = isFarAway ? TreeForms.StandInFor(form) : form;
            return byForm[shown][level];
        }

        private static Dictionary<DetailLevel, TreeModel> ModelsOf(TreeForm form)
        {
            var figures = TreeFigures.Of(form);
            return figures.ToDictionary(figure => figure.Key, figure => new TreeModel(figure.Value, form.Species));
        }
    }
}
