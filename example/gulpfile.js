const gulp          = require('gulp')
const foreach       = require("gulp-foreach")
const rename        = require('gulp-rename')
const handlebars    = require('gulp-compile-handlebars')

//
const _exec         = require('child_process').execSync

gulp.task('codegen', () => {
    return gulp.src('../Habitat/src/**/unicorn.json')
        .pipe(foreach((stream, file) => {
            let config = JSON.parse(file.contents.toString())
            let helpers = require(config.helpers)

            // Generate the models :)
            let out = _exec(config.generator + ' -f ' + config.source + ' -p "\\sitecore\\templates"')

            var content = JSON.parse(out)

            return gulp.src(config.template)
                .pipe(handlebars({ models: content }, { helpers: helpers }))
                .pipe(rename(config.filename))
                .pipe(gulp.dest(config.destination))
        }))
        .on('error', (error) => console.log(error));
});

gulp.task('default', ['codegen'],  () => { })