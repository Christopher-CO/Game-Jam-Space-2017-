"# spacejam2017" 

Helpful commands: 

GitHub doesn't let you push files to repo >100MB in size. 
Need to make sure when you commit that you're ignoring super-big files.

Use: 
find . -size +100M | sed 's|^\./||g' | cat >> .gitignore; awk '!NF || !seen[$0]++' .gitignore

See: http://stackoverflow.com/questions/4035779/gitignore-by-file-size