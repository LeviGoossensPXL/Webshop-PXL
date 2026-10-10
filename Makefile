start:
	podman compose up website.database webapi.database -Vd
	

podman-start:
	podman compose down
	podman compose up -Vd

podman-build:
	podman compose build --build-arg BUILD_CONFIGURATION=Debug

podman-build-no-cache:
	podman compose build --build-arg BUILD_CONFIGURATION=Debug --no-cache

podman-remove:
	podman compose down --remove-orphans
	podman container prune -f
	podman image prune -af --external
	podman volume prune -f