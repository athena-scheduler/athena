const path = require("path");
const webpack = require("webpack");
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const CleanWebpackPlugin = require("clean-webpack-plugin");
const CopyPlugin = require("copy-webpack-plugin");

const wwwroot = path.resolve(__dirname, "wwwroot");

module.exports = {
    entry: {
        athena: './bundle.js',
        studentSetup: './Content/js/studentSetup.js',
        schedule: './Content/js/schedule.js',
        completedCourses: './Content/js/ConfigureCompletedCourses.js',
        print: './Content/css/print.css'
    },
    output: {
        filename: "[name].js",
        path: wwwroot,
        libraryTarget: 'var',
        library: ['athena', '[name]']
    },
    resolve: {
        alias: {
            jquery: 'jquery/src/jquery'
        }
    },
    module: {
        rules: [
            {
                test: require.resolve('jquery'),
                use: [
                    {
                        loader: 'expose-loader',
                        options: '$'
                    },
                    {
                        loader: 'expose-loader',
                        options: 'jquery'
                    },
                    {
                        loader: 'expose-loader',
                        options: 'jQuery'
                    }
                ]
            },
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
        new ExtractTextPlugin({ filename: '[name].css', allChunks: true }),
    ]
};