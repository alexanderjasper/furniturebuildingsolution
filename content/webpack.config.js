const path = require("path");
const webpack = require("webpack");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const OptimizeCSSPlugin = require("optimize-css-assets-webpack-plugin");
const TerserPlugin = require("terser-webpack-plugin");
const bundleOutputDir = "./wwwroot/dist";
const VueLoaderPlugin = require("vue-loader/lib/plugin");
const CompressionWebpackPlugin = require("compression-webpack-plugin");
const BundleAnalyzerPlugin = require("webpack-bundle-analyzer")
  .BundleAnalyzerPlugin;

module.exports = () => {
  console.log("Building for " + process.env.NODE_ENV);

  const isDevBuild = !(
    process.env.NODE_ENV && process.env.NODE_ENV === "production"
  );

  const extractCSS = new MiniCssExtractPlugin({
    filename: "site.css"
  });

  return [
    {
      mode: isDevBuild ? "development" : "production",
      stats: { modules: false },
      entry: { main: "./ClientApp/boot-app.js" },
      resolve: {
        extensions: [".js", ".vue"],
        alias: isDevBuild
          ? {
              vue$: "vue/dist/vue",
              components: path.resolve(__dirname, "./ClientApp/components"),
              views: path.resolve(__dirname, "./ClientApp/views"),
              utils: path.resolve(__dirname, "./ClientApp/utils"),
              api: path.resolve(__dirname, "./ClientApp/store/api")
            }
          : {
              components: path.resolve(__dirname, "./ClientApp/components"),
              views: path.resolve(__dirname, "./ClientApp/views"),
              utils: path.resolve(__dirname, "./ClientApp/utils"),
              api: path.resolve(__dirname, "./ClientApp/store/api")
            }
      },
      output: {
        path: path.join(__dirname, bundleOutputDir),
        filename: "[name].js",
        publicPath: "/dist/",
        chunkFilename: "[id]-[chunkhash].js"
      },
      module: {
        rules: [
          { test: /\.vue$/, include: /ClientApp/, use: "vue-loader" },
          { test: /\.js$/, include: /ClientApp/, use: "babel-loader" },
          {
            test: /\.css$/,
            use: isDevBuild
              ? ["style-loader", "css-loader"]
              : [MiniCssExtractPlugin.loader, "css-loader"]
          },
          { test: /\.(png|jpg|jpeg|gif|svg)$/, use: "url-loader?limit=25000" }
        ]
      },
      plugins: [
        // new BundleAnalyzerPlugin(),
        new VueLoaderPlugin(),
        new webpack.DllReferencePlugin({
          context: __dirname,
          manifest: require("./wwwroot/dist/vendor-manifest.json")
        })
      ].concat(
        isDevBuild
          ? [
              // Plugins that apply in development builds only
              new webpack.SourceMapDevToolPlugin({
                filename: "[file].map", // Remove this line if you prefer inline source maps
                moduleFilenameTemplate: path.relative(
                  bundleOutputDir,
                  "[resourcePath]"
                ) // Point sourcemap entries to the original file locations on disk
              })
            ]
          : [
              extractCSS,
              // Compress extracted CSS.
              new OptimizeCSSPlugin({
                cssProcessorOptions: {
                  safe: true
                }
              }),
              new CompressionWebpackPlugin({
                algorithm: "gzip"
              })
            ]
      ),
      optimization: {
        minimizer: [
          new TerserPlugin({
            cache: true,
            parallel: true,
            terserOptions: {
              toplevel: true,
              safari10: true
            },
            sourceMap: isDevBuild
          })
        ]
      },
      devtool: "#eval-source-map",
      externals: {
        // global app config object
        config: JSON.stringify({
          apiUrl: isDevBuild
            ? "http://localhost:5001"
            : "https://test.shelfer.dk"
        })
      }
    }
  ];
};
