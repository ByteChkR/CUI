using System.Collections;
using System.Numerics;

using CUI.Common;
using CUI.Common.Components;

namespace CUI.Console;

internal class Program
{
    private static RenderContext RenderContext { get; } = new RenderContext(new ConsoleRenderer());

    private static IEnumerator MoveTest(Renderable renderable, bool even)
    {
        Vector2 offset = new Vector2(1, even ? 1 : -1);
        for (int i = 0; i < 10; i++)
        {
            renderable.Transform.Position += offset;
            renderable.Transform.ZIndex = i % 2 == 0 == even ? 1 : 0;
            if(i % 2 == 0 == even)
            {
                renderable.BackgroundColor = RenderColor.Red;
                renderable.ForegroundColor = RenderColor.Green;
            }
            else
            {
                renderable.BackgroundColor = RenderColor.Green;
                renderable.ForegroundColor = RenderColor.Red;
            }

            yield return null;
        }
    }

    private static void Loop(params IEnumerator[] animations)
    {
        bool completed = false;

        while (!completed)
        {
            completed = true;

            for (int i = 0; i < animations.Length; i++)
            {
                IEnumerator animation = animations[i];
                if (animation.MoveNext())
                {
                    completed = false;
                }
            }
            RenderContext.Render();
            Thread.Sleep(100);
        }
    }
    
    private static void Main(string[] args)
    {
        Renderable text1 = new BorderedPanel(BorderOptions.Default)
            .WithChild(new Text("Hello World!")
                           .WithSize(12, 1)
                      )
            .WithPadding(3, 1)
            .WithSize(6, 1)
            .WithColors(RenderColor.Red, RenderColor.Green)
            .WithOverflow(OverflowMode.Hidden);
        Renderable text2 = new BorderedPanel(BorderOptions.Double)
            .WithChild(new Text("Hello World!")
                           .WithLayout(LayoutMode.Fit)
                      )
            .AtPosition(5, 10)
            .WithPadding(3, 1)
            .WithSize(6, 1)
            .WithColors(RenderColor.Green, RenderColor.Red)
            .WithLayout(LayoutMode.Fit);
        RenderContext.Elements.WithChildren(text1, text2);
        RenderContext.Render();
        Loop(MoveTest(text1, true), MoveTest(text2, false));
    }
}