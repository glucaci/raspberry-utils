FROM armhf/debian:jessie

# Mopidy configuration
COPY mopidy.conf /var/lib/mopidy/.config/mopidy/mopidy.conf

# Start helper script
COPY entrypoint.sh /entrypoint.sh

RUN set -ex \
 && apt-get update \
 && DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends \
	dbus \
	apt-utils \
	curl \
	gcc \
	python-dev

# Install Mopidy
RUN set -ex \
	&& curl -L https://apt.mopidy.com/mopidy.gpg -o /tmp/mopidy.gpg \
	&& curl -L https://apt.mopidy.com/mopidy.list -o /etc/apt/sources.list.d/mopidy.list \
	&& apt-key add /tmp/mopidy.gpg \
	&& apt-get update \
	&& DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends mopidy
 
# Install Mopidy Backend Extensions	
RUN set -ex \
	&& apt-get install -y --no-install-recommends libspotify-dev \
	&& curl -L https://bootstrap.pypa.io/get-pip.py | python - \
	&& pip install Mopidy-Spotify \
	&& pip install Mopidy-Local-SQLite
 
# Install Mopidy Frontend Extensions
RUN set -ex \
	&& pip install Mopidy-Iris

# Install dumb-init
RUN set -ex \
	&& DUMP_INIT_URI=$(curl -L https://github.com/Yelp/dumb-init/releases/latest | grep -Po '(?<=href=")[^"]+_amd64(?=")') \
	&& curl -Lo /usr/local/bin/dumb-init "https://github.com/$DUMP_INIT_URI" \
	&& chmod +x /usr/local/bin/dumb-init

# Clean-up
RUN set -ex \
	&& apt-get purge --auto-remove -y \
		libspotify-dev \
		python-dev \
		gcc \
        curl \
		apt-utils \
	&& apt-get clean \
	&& rm -rf /var/lib/apt/lists/* /tmp/* /var/tmp/* ~/.cache

# Setup
RUN set -ex \
	&& chown mopidy:audio -R /var/lib/mopidy/.config \
	&& chown mopidy:audio /entrypoint.sh

USER mopidy
VOLUME ["/var/lib/mopidy/local", "/var/lib/mopidy/media"]
EXPOSE 6600 6680
ENTRYPOINT ["/usr/local/bin/dumb-init", "/bin/bash"]
