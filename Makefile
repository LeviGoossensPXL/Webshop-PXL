start:
	podman compose up -V -d

build:
	podman compose build --no-cache

remove-all:
	podman compose down --remove-orphans
	podman container prune -f
	podman image prune -a --external
	podman volume prune -f