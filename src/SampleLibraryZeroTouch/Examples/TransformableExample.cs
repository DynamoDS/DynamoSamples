using Autodesk.DesignScript.Geometry;
using Autodesk.DesignScript.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examples
{
    /// <summary>
    /// An object which knows how to draw itself in the background preview and uses a transform to take 
    /// advantage of the GPU to alter that background visualization. The original geometry remains unaltered,
    /// only the visualization is transformed.
    /// </summary>
    public class TransformableExample : IGraphicItem
    {
        public Geometry Geometry { get; private set; }
        private CoordinateSystem transform { get; set; }

        //want to hide this constructor
        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]
        public TransformableExample(Geometry geometry)
        {
            Geometry = geometry;
            //initial transform is just at the origin
            transform = CoordinateSystem.ByOrigin(0, 0, 0);
        }

     /// <summary>
     /// Create a TranformableExample class which stores a Geometry object and a Transform.
     /// </summary>
     /// <param name="geometry"> a geometry object</param>
     /// <returns></returns>
        public static TransformableExample ByGeometry(Autodesk.DesignScript.Geometry.Geometry geometry)
        {
            var newTransformableThing = new TransformableExample(geometry);
            return newTransformableThing;
        }

        /// <summary>
        /// This method sets the transform on the object and returns a reference to the object so
        /// the tessellate method is called and the new visualization shows in the background preview.
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public TransformableExample TransformObject(CoordinateSystem transform)
        {
            this.transform = transform;
            return this;
        }

        /// <summary>
        /// This method is actually called by Dynamo when it attempts to render the TransformableExample. 
        /// class.
        /// </summary>
        /// <param name="package"></param>
        /// <param name="parameters"></param>
        //hide this method from search
        [Autodesk.DesignScript.Runtime.IsVisibleInDynamoLibrary(false)]

        public void Tessellate(IRenderPackage package, TessellationParameters parameters)
        {
            //could increase performance further by cacheing this tesselation
            Geometry.Tessellate(package, parameters);

            //we use reflection here because this API was added in Dynamo 1.1 and might not exist for a user in Dynamo 1.0
            //if you do not care about ensuring comptability of your zero touch node with 1.0 you can just call SetTransform directly
            //by casting the rendering package to ITransformable.
            
            //look for the method SetTransform with the double[] argument list.
            var method = package.GetType().
            GetMethod("SetTransform", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance,
                    null,
                    new[] { typeof(double[]) }, null);

            //if the method exists call it using our current transform.
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

    public static class PeriodicIncrement
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
