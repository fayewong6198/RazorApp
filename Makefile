build:
	docker build -t razorapp .

check-image:
	docker images

run:
	docker run -it --rm -p 3000:80 --name razorappcontainer razorapp