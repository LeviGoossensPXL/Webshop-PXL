start:
	podman compose down
	podman compose up -Vd

build:
	podman compose build

build-no-cache:
	podman compose build --no-cache

remove-all:
	podman compose down --remove-orphans
	podman container prune -f
	podman image prune -af --external
	podman volume prune -f