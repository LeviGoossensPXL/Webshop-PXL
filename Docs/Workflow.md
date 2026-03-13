# workflow
guide and rules on how we work together on this codebase.

## rules (try to follow as best as possible)
1. always push your changes.
	- Commit regularly and push your work to the remote repository.
2. make a new branch on azure devops for your issue.
	- When starting work on an issue, create a dedicated branch for it.
	- example: `38-create-web-api`
	- See the screenshots below for where to create the branch.
![img1](./images/Screenshot_2026-03-13_203647.png)
![img2](./images/Screenshot_2026-03-13_203940.png)
3. only try to make changes related to your issue (to minimize merge conflicts)
4. Push your branch and open a PR to the dev branch.
5. Request a review before merging
	- At least one team member should review the PR.
	- Address any feedback before merging. with a comment or a code fix.
6. Make sure the project still builds/runs
	- Test your changes locally before pushing.
	- Ensure no existing functionality is broken.