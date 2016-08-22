using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleLibraryZeroTouch.Examples
{
    public class TransformableExample : IGraphicItem
    {
        public Geometry Geometry { get; private set; }
        private CoordinateSystem transform { get; set; }
     /// <summary>
     /// 
     /// </summary>
     /// <param name="geometry"> a geometry object</param>
     /// <returns></returns>
        public static TransformableExample byGeometry(Autodesk.DesignScript.Geometry.Geometry geometry)
        {
            var newTransformableThing = new TransformableExample()
            {
                Geometry = geometry,
                transform = CoordinateSystem.ByOrigin(0, 0, 0)
            };

            return newTransformableThing;
        }

        public TransformableExample TransformObject(CoordinateSystem transform)
        {
            this.transform = transform;
            return this;
        }

        public void Tessellate(IRenderPackage package, TessellationParameters parameters)
        {
            //could increase performance by cacheing this tesselation
            Geometry.Tessellate(package, parameters);

            //we use reflection here because this API was added in Dynamo 1.1 and might not exist for a user in Dynamo 1.0
            //if you do not care about ensuring comptability of your zero touch node with 1.0 you can just call SetTransform directly
            //by casting the rendering package to ITransformable.
            //look for the method SetTransform with the double[] argument list.
            var method = package.GetType().
            GetMethod("SetTransform", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance,
                    null,
                    new[] { typeof(double[]) }, null);

            //if the method exists call it using our transform.
            if (method != null)
            {
                method.Invoke(package, new object[] { new double[]
        {transform.XAxis.X,transform.XAxis.Y,transform.XAxis.Z,0,
        transform.YAxis.X,transform.YAxis.Y,transform.YAxis.Z,0,
        transform.ZAxis.X,transform.ZAxis.Y,transform.ZAxis.Z,0,
        transform.Origin.X,transform.Origin.Y,transform.Origin.Z,1
        }});
            }

        }

    }

   //TODO(change this to fader);
    public class PeriodicIncrement
    {
        private static double value = 0;
       
        [Autodesk.DesignScript.Runtime.CanUpdatePeriodically(true)]
        public static double Increment()
        {
            value = value + 1;
            return value;
        }
    }

}
