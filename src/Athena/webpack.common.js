const path = require("path");
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const CleanWebpackPlugin = require("clean-webpack-plugin");
const CopyPlugin = require("copy-webpack-plugin");

const wwwroot = path.resolve(__dirname, "wwwroot");

module.exports = {
    entry: "./bundle.js",
    output: {
        filename: "athena.js",
        path: wwwroot
    },
    module: {
        rules: [
            {
                test: /\.css$/,
                use: ExtractTextPlugin.extract({
                    fallback: "style-loader",
                    use: "css-loader"
                })
            },
            {
                test: /\.(png|svg|jpg|gif)$/,
                use: ["file-loader"]
            },
            {
                test: /\.(woff|woff2|eot|ttf|otf)$/,
                use: ["file-loader"]
            }
        ]
    },
    plugins: [
        new CleanWebpackPlugin([wwwroot]),
        new CopyPlugin([{context: "./Content/images", from: "**/*", to: path.join(wwwroot, "images")}]),
        new ExtractTextPlugin("athena.css"),
    ]
};