using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FurnitureBuildingSolution.Helpers
{
    public class SkyCivJsonInputObject
    {
        public class Rootobject
        {
            public Auth auth { get; set; }
            public File_Management file_management { get; set; }
            public Settings settings { get; set; }
            public ExpandoObject nodes { get; set; }
            public ExpandoObject members { get; set; }
            public ExpandoObject sections { get; set; }
            public ExpandoObject materials { get; set; }
            public ExpandoObject supports { get; set; }
            public ExpandoObject distributed_loads { get; set; }
            public Self_Weight self_weight { get; set; }
        }

        public class Auth
        {
            public string username { get; set; }
            public string key { get; set; }
        }

        public class File_Management
        {
            public bool save_file { get; set; }
        }

        public class Settings
        {
            public int analysis_type { get; set; }
            public bool accurate_buckling_shape { get; set; }
            public string units { get; set; }
            public int precision { get; set; }
            public int precision_values { get; set; }
            public int evaluation_points { get; set; }
            public int non_linear_tolerance { get; set; }
            public bool auto_stabilize_model { get; set; }
            public bool analysis_report { get; set; }
        }

        public class Node
        {
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }
        }

        public class Member
        {
            public string type { get; set; }
            public int node_A { get; set; }
            public int node_B { get; set; }
            public int section_id { get; set; }
            public int rotation_angle { get; set; }
            public string fixity_A { get; set; }
            public string fixity_B { get; set; }
            public int offset_Ax { get; set; }
            public int offset_Ay { get; set; }
            public int offset_Az { get; set; }
            public int offset_Bx { get; set; }
            public int offset_By { get; set; }
            public int offset_Bz { get; set; }
        }

        public class Section
        {
            public string name { get; set; }
            public int area { get; set; }
            public int Iy { get; set; }
            public int Iz { get; set; }
            public int J { get; set; }
            public int material_id { get; set; }
        }

        public class Material
        {
            public string name { get; set; }
            public int density { get; set; }
            public int elasticity_modulus { get; set; }
            public double poissons_ratio { get; set; }
        }

        public class Support
        {
            public int node { get; set; }
            public string restraint_code { get; set; }
            public int tx { get; set; }
            public int ty { get; set; }
            public int tz { get; set; }
            public int rx { get; set; }
            public int ry { get; set; }
            public int rz { get; set; }
        }

        public class Distributed_Load
        {
            public int member { get; set; }
            public double x_mag_A { get; set; }
            public double y_mag_A { get; set; }
            public double z_mag_A { get; set; }
            public double x_mag_B { get; set; }
            public double y_mag_B { get; set; }
            public double z_mag_B { get; set; }
            public int position_A { get; set; }
            public int position_B { get; set; }
            public int load_group { get; set; }
            public string axes { get; set; }
        }

        public class Self_Weight
        {
            public bool enabled { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int z { get; set; }
        }

    }
}
